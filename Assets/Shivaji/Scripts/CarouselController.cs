using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarouselController : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;
    public RectTransform viewport;
    public RectTransform content;

    [Header("Edge Spacers")]
    public bool useEdgeSpacers = true;
    public string leftSpacerName = "LeftSpacer";
    public string rightSpacerName = "RightSpacer";
    public float extraEdgePadding = 120f; // ✅ Little increase (50–300)

    [Header("Scaling Settings")]
    public float centerScale = 1.25f;
    public float scaleRange = 250f;

    [Header("Snapping Settings")]
    public float snapTime = 0.15f;
    public float snapVelocityThreshold = 80f;
    public float snapDelayAfterDrag = 0.05f;

    [Header("Debug")]
    public bool enableDebug = true;

    bool isSnapping;

    Vector2 targetPosition;
    Vector2 snapVelocity;

    float lastDragStartX;
    public float movedEpsilon = 0.5f;

    Coroutine snapRoutine;

    RectTransform leftSpacer;
    RectTransform rightSpacer;
    LayoutElement leftSpacerLayout;
    LayoutElement rightSpacerLayout;

    void Reset()
    {
        if (!scrollRect) scrollRect = GetComponent<ScrollRect>();
        if (scrollRect)
        {
            if (!content) content = scrollRect.content;
            if (!viewport) viewport = scrollRect.viewport;
        }
    }

    void Awake()
    {
        if (!scrollRect) scrollRect = GetComponent<ScrollRect>();
        if (scrollRect && !content) content = scrollRect.content;
        if (scrollRect && !viewport) viewport = scrollRect.viewport;

        if (scrollRect) scrollRect.movementType = ScrollRect.MovementType.Clamped;
    }

    IEnumerator Start()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();

        if (content)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            Canvas.ForceUpdateCanvases();
        }

        if (useEdgeSpacers)
        {
            EnsureSpacers();
            UpdateSpacerSizes();
        }
    }

    void OnRectTransformDimensionsChange()
    {
        if (!isActiveAndEnabled) return;
        if (!useEdgeSpacers) return;
        if (!content || !viewport) return;

        UpdateSpacerSizes();
    }

    void Update()
    {
        if (!scrollRect || !viewport || !content) return;

        Vector3 viewportCenterWorld = viewport.TransformPoint(viewport.rect.center);
        Vector3 viewportCenterInContent = content.InverseTransformPoint(viewportCenterWorld);

        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            if (!child) continue;

            if (useEdgeSpacers && (child == leftSpacer || child == rightSpacer))
                continue;

            Vector3 childCenterWorld = child.TransformPoint(child.rect.center);
            Vector3 childCenterInContent = content.InverseTransformPoint(childCenterWorld);

            float distance = Mathf.Abs(viewportCenterInContent.x - childCenterInContent.x);
            float t = Mathf.Clamp01(distance / scaleRange);

            float scale = Mathf.Lerp(centerScale, 1f, t);
            child.localScale = Vector3.one * scale;

            CanvasGroup group = child.GetComponent<CanvasGroup>();
            if (group != null) group.alpha = Mathf.Lerp(1f, 0.4f, t);
        }

        if (isSnapping)
        {
            content.anchoredPosition = Vector2.SmoothDamp(
                content.anchoredPosition,
                targetPosition,
                ref snapVelocity,
                snapTime
            );

            if (Vector2.Distance(content.anchoredPosition, targetPosition) < 0.1f)
            {
                content.anchoredPosition = targetPosition;
                isSnapping = false;

                if (enableDebug)
                    Debug.Log($"[Carousel] Snap finished. finalX:{content.anchoredPosition.x:0.00}");
            }
        }
    }

    public void OnBeginDrag()
    {
        isSnapping = false;
        lastDragStartX = content.anchoredPosition.x;

        if (snapRoutine != null)
        {
            StopCoroutine(snapRoutine);
            snapRoutine = null;
        }

        if (enableDebug)
            Debug.Log($"[Carousel] BeginDrag contentX:{content.anchoredPosition.x:0.00}");
    }

    public void OnEndDrag()
    {
        if (enableDebug)
            Debug.Log($"[Carousel] EndDrag contentX:{content.anchoredPosition.x:0.00} vel:{scrollRect.velocity.magnitude:0.00}");

        if (snapRoutine != null) StopCoroutine(snapRoutine);
        snapRoutine = StartCoroutine(SnapWhenSettled());
    }

    IEnumerator SnapWhenSettled()
    {
        yield return new WaitForSeconds(snapDelayAfterDrag);

        while (scrollRect.velocity.magnitude > snapVelocityThreshold)
            yield return null;

        scrollRect.velocity = Vector2.zero;
        SnapToNearest();
    }

    void SnapToNearest()
    {
        if (content.childCount == 0) return;

        Vector3 viewportCenterWorld = viewport.TransformPoint(viewport.rect.center);
        Vector3 viewportCenterInContent = content.InverseTransformPoint(viewportCenterWorld);

        float closest = float.MaxValue;
        RectTransform closestChild = null;
        float closestChildCenterX = 0f;

        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            if (!child) continue;

            if (useEdgeSpacers && (child == leftSpacer || child == rightSpacer))
                continue;

            Vector3 childCenterWorld = child.TransformPoint(child.rect.center);
            Vector3 childCenterInContent = content.InverseTransformPoint(childCenterWorld);

            float distance = Mathf.Abs(viewportCenterInContent.x - childCenterInContent.x);
            if (distance < closest)
            {
                closest = distance;
                closestChild = child;
                closestChildCenterX = childCenterInContent.x;
            }
        }

        if (!closestChild) return;

        float deltaX = viewportCenterInContent.x - closestChildCenterX;

        Vector2 rawTarget = content.anchoredPosition + new Vector2(deltaX, 0f);
        targetPosition = ClampByFirstLastCentered(rawTarget);

        snapVelocity = Vector2.zero;
        isSnapping = true;

        if (enableDebug)
        {
            Debug.Log(
                $"[Carousel] Closest:{closestChild.name} dist:{closest:0.00} " +
                $"vpX:{viewportCenterInContent.x:0.00} childX:{closestChildCenterX:0.00} " +
                $"deltaX:{deltaX:0.00} rawTargetX:{rawTarget.x:0.00} targetX:{targetPosition.x:0.00} contentX:{content.anchoredPosition.x:0.00}"
            );
        }
    }

    Vector2 ClampByFirstLastCentered(Vector2 desired)
    {
        RectTransform first = GetFirstRealItem();
        RectTransform last = GetLastRealItem();
        if (!first || !last) return desired;

        Vector3 vpCenterWorld = viewport.TransformPoint(viewport.rect.center);
        float vpX = content.InverseTransformPoint(vpCenterWorld).x;

        float firstX = content.InverseTransformPoint(first.TransformPoint(first.rect.center)).x;
        float lastX = content.InverseTransformPoint(last.TransformPoint(last.rect.center)).x;

        // content position if first is centered
        float xFirstCentered = content.anchoredPosition.x + (vpX - firstX);

        // content position if last is centered
        float xLastCentered = content.anchoredPosition.x + (vpX - lastX);

        float minX = Mathf.Min(xFirstCentered, xLastCentered);
        float maxX = Mathf.Max(xFirstCentered, xLastCentered);

        desired.x = Mathf.Clamp(desired.x, minX, maxX);
        return desired;
    }



    void EnsureSpacers()
    {
        leftSpacer = content.Find(leftSpacerName) as RectTransform;
        rightSpacer = content.Find(rightSpacerName) as RectTransform;

        if (!leftSpacer) leftSpacer = CreateSpacer(leftSpacerName);
        if (!rightSpacer) rightSpacer = CreateSpacer(rightSpacerName);

        leftSpacer.SetAsFirstSibling();
        rightSpacer.SetAsLastSibling();

        leftSpacerLayout = leftSpacer.GetComponent<LayoutElement>();
        rightSpacerLayout = rightSpacer.GetComponent<LayoutElement>();

        if (!leftSpacerLayout) leftSpacerLayout = leftSpacer.gameObject.AddComponent<LayoutElement>();
        if (!rightSpacerLayout) rightSpacerLayout = rightSpacer.gameObject.AddComponent<LayoutElement>();

        leftSpacerLayout.ignoreLayout = false;
        rightSpacerLayout.ignoreLayout = false;
    }

    RectTransform CreateSpacer(string spacerName)
    {
        GameObject go = new GameObject(spacerName, typeof(RectTransform));
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.SetParent(content, false);
        rt.localScale = Vector3.one;
        rt.anchorMin = new Vector2(0f, 0.5f);
        rt.anchorMax = new Vector2(0f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        Image img = go.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0f);

        return rt;
    }

    void UpdateSpacerSizes()
    {
        if (!content || !viewport) return;

        EnsureSpacers();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        Canvas.ForceUpdateCanvases();

        RectTransform firstItem = GetFirstRealItem();
        RectTransform lastItem = GetLastRealItem();

        if (!firstItem || !lastItem) return;

        float viewportWidth = viewport.rect.width;
        float firstWidth = firstItem.rect.width;
        float lastWidth = lastItem.rect.width;

        float leftPad = Mathf.Max(0f, (viewportWidth * 0.5f) - (firstWidth * 0.5f));
        float rightPad = Mathf.Max(0f, (viewportWidth * 0.5f) - (lastWidth * 0.5f));

        // ✅ Little increase on both sides
        leftPad += extraEdgePadding;
        rightPad += extraEdgePadding;

        leftSpacerLayout.preferredWidth = leftPad;
        rightSpacerLayout.preferredWidth = rightPad;

        if (enableDebug)
            Debug.Log($"[Carousel] SpacerSizes left:{leftPad:0.00} right:{rightPad:0.00} extra:{extraEdgePadding:0.00} vp:{viewportWidth:0.00} first:{firstWidth:0.00} last:{lastWidth:0.00}");

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        Canvas.ForceUpdateCanvases();
    }

    RectTransform GetFirstRealItem()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            if (!child) continue;
            if (useEdgeSpacers && (child == leftSpacer || child == rightSpacer)) continue;
            return child;
        }
        return null;
    }

    RectTransform GetLastRealItem()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            if (!child) continue;
            if (useEdgeSpacers && (child == leftSpacer || child == rightSpacer)) continue;
            return child;
        }
        return null;
    }
}
