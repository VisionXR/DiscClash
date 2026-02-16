using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarouselController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;

    [Header("Spacers")]
    [SerializeField] private LayoutElement leftSpacer;
    [SerializeField] private LayoutElement rightSpacer;

    [Header("Snap Settings")]
    [SerializeField] private float snapDelayAfterDrag = 0.05f;
    [SerializeField] private float snapVelocityThreshold = 20f;
    [SerializeField] private float snapLerpSpeed = 12f;

    [Header("Padding Extra")]
    [SerializeField] private float extraSpacerPadding = 120f;

    [Header("Debug")]
    [SerializeField] private bool enableDebug;

    private Coroutine snapCoroutine;
    private float dragStartContentX;

    private void Reset()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        if (scrollRect != null)
        {
            viewport = scrollRect.viewport;
            content = scrollRect.content;
        }
    }

    private IEnumerator Start()
    {
        // Wait for layout to build
        yield return null;
        Canvas.ForceUpdateCanvases();
        UpdateSpacerSizes();
    }

    private void OnEnable()
    {
        Canvas.willRenderCanvases += OnWillRenderCanvases;
    }

    private void OnDisable()
    {
        Canvas.willRenderCanvases -= OnWillRenderCanvases;
    }

    private void OnWillRenderCanvases()
    {
        // Helps when resolution / safe-area / scaler changes
        UpdateSpacerSizes();
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateSpacerSizes();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartContentX = content.anchoredPosition.x;

        if (snapCoroutine != null)
        {
            StopCoroutine(snapCoroutine);
            snapCoroutine = null;
        }

        if (enableDebug)
            Debug.Log($"[Carousel] BeginDrag contentX:{content.anchoredPosition.x:0.00}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (enableDebug)
            Debug.Log($"[Carousel] EndDrag contentX:{content.anchoredPosition.x:0.00} vel:{scrollRect.velocity.x:0.00}");

        if (snapCoroutine != null)
        {
            StopCoroutine(snapCoroutine);
            snapCoroutine = null;
        }

        snapCoroutine = StartCoroutine(SnapWhenSettled());
    }

    private IEnumerator SnapWhenSettled()
    {
        // Small delay so inertia begins
        yield return new WaitForSeconds(snapDelayAfterDrag);

        // Wait until scroll velocity is low enough (settled)
        while (scrollRect.velocity.magnitude > snapVelocityThreshold)
            yield return null;

        // IMPORTANT: Always snap (do NOT skip when moved too little)
        scrollRect.velocity = Vector2.zero;
        SnapToNearest();
    }

    private void SnapToNearest()
    {
        if (viewport == null || content == null)
            return;

        float vpCenterX = GetViewportCenterInContentSpaceX();

        RectTransform closest = null;
        float closestDist = float.MaxValue;

        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            if (child == null || !child.gameObject.activeInHierarchy)
                continue;

            // Ignore spacers if they are part of content children
            if (leftSpacer != null && child.gameObject == leftSpacer.gameObject) continue;
            if (rightSpacer != null && child.gameObject == rightSpacer.gameObject) continue;

            float childCenterX = GetChildCenterXInContentSpace(child);
            float dist = Mathf.Abs(vpCenterX - childCenterX);

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = child;
            }
        }

        if (closest == null)
            return;

        float targetContentX = GetTargetContentXToCenterChild(closest);

        if (enableDebug)
        {
            float childX = GetChildCenterXInContentSpace(closest);
            Debug.Log($"[Carousel] Closest:{closest.name} dist:{closestDist:0.00} vpX:{vpCenterX:0.00} childX:{childX:0.00} targetX:{targetContentX:0.00} contentX:{content.anchoredPosition.x:0.00}");
        }

        if (snapCoroutine != null)
            StopCoroutine(snapCoroutine);

        snapCoroutine = StartCoroutine(LerpToX(targetContentX));
    }

    private IEnumerator LerpToX(float targetX)
    {
        float startX = content.anchoredPosition.x;
        float t = 0f;

        while (Mathf.Abs(content.anchoredPosition.x - targetX) > 0.1f)
        {
            t += Time.unscaledDeltaTime * snapLerpSpeed;
            float newX = Mathf.Lerp(startX, targetX, t);
            content.anchoredPosition = new Vector2(newX, content.anchoredPosition.y);
            yield return null;
        }

        content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);

        if (enableDebug)
            Debug.Log($"[Carousel] Snap finished. finalX:{targetX:0.00}");

        snapCoroutine = null;
    }

    public void UpdateSpacerSizes()
    {
        if (viewport == null || content == null || layoutGroup == null || leftSpacer == null || rightSpacer == null)
            return;

        Canvas.ForceUpdateCanvases();

        float vpWidth = viewport.rect.width;

        RectTransform first = GetFirstRealChild();
        RectTransform last = GetLastRealChild();

        if (first == null || last == null)
            return;

        float firstWidth = first.rect.width;
        float lastWidth = last.rect.width;

        float baseLeft = Mathf.Max(0f, (vpWidth - firstWidth) * 0.5f);
        float baseRight = Mathf.Max(0f, (vpWidth - lastWidth) * 0.5f);

        float left = baseLeft + extraSpacerPadding;
        float right = baseRight + extraSpacerPadding;

        leftSpacer.preferredWidth = left;
        rightSpacer.preferredWidth = right;

        if (enableDebug)
            Debug.Log($"[Carousel] SpacerSizes left:{left:0.00} right:{right:0.00} extra:{extraSpacerPadding:0.00} vp:{vpWidth:0.00} first:{firstWidth:0.00} last:{lastWidth:0.00}");

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    private RectTransform GetFirstRealChild()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            if (child == null || !child.gameObject.activeInHierarchy) continue;
            if (leftSpacer != null && child.gameObject == leftSpacer.gameObject) continue;
            if (rightSpacer != null && child.gameObject == rightSpacer.gameObject) continue;
            return child;
        }
        return null;
    }

    private RectTransform GetLastRealChild()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            RectTransform child = content.GetChild(i) as RectTransform;
            if (child == null || !child.gameObject.activeInHierarchy) continue;
            if (leftSpacer != null && child.gameObject == leftSpacer.gameObject) continue;
            if (rightSpacer != null && child.gameObject == rightSpacer.gameObject) continue;
            return child;
        }
        return null;
    }

    private float GetViewportCenterInContentSpaceX()
    {
        // viewport center in its local space
        float vpLocalCenter = viewport.rect.width * 0.5f;

        // Convert viewport-local x to content-local x:
        // content anchoredPosition.x shifts content relative to viewport
        // so viewport center in content space is:
        return -content.anchoredPosition.x + vpLocalCenter;
    }

    private float GetChildCenterXInContentSpace(RectTransform child)
    {
        // Child position is relative to content pivot/anchors
        // Take child local x + half width (center)
        return child.anchoredPosition.x + (child.rect.width * 0.5f);
    }

    private float GetTargetContentXToCenterChild(RectTransform child)
    {
        float vpWidth = viewport.rect.width;
        float vpCenterLocal = vpWidth * 0.5f;

        float childCenter = GetChildCenterXInContentSpace(child);

        // We want: viewportCenterInContentSpace == childCenter
        // viewportCenterInContentSpace = -contentX + vpCenterLocal
        // => -contentX + vpCenterLocal = childCenter
        // => contentX = vpCenterLocal - childCenter
        return vpCenterLocal - childCenter;
    }
}
