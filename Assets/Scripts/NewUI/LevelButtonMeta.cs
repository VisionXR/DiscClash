using UnityEngine;
using UnityEngine.UI;

public class LevelButtonMeta : MonoBehaviour
{
    public int levelNo = 0;                 // 0-based
    public GameObject lockGO;
    [HideInInspector] public Button button;

    public TrickShotPanel panel;

    [Header("Stars UI")]
    [Tooltip("Parent that holds exactly Star1, Star2, Star3 (each with a CanvasGroup).")]
    public Transform starsRoot;

    [Tooltip("Leave empty to auto-fill from starsRoot’s DIRECT children (order: left->right).")]
    public CanvasGroup[] starCGs;

    [Tooltip("If true, hide stars that aren't earned. If false, dim them instead.")]
    public bool hideUnlitStars = true;
    [Range(0f, 1f)] public float dimAlpha = 0.2f;
    [Range(0f, 1f)] public float litAlpha = 1f;

    private void Awake()
    {
        if (!button)
        {
            button = GetComponent<Button>();
            if (!button) button = GetComponentInChildren<Button>(true);
        }
        EnsureStarRefs();
    }

    private void OnEnable()
    {
        if (!panel) panel = Object.FindFirstObjectByType<TrickShotPanel>();
        if (button != null)
        {
            button.onClick.RemoveListener(InvokeLevel);
            button.onClick.AddListener(InvokeLevel);
        }
    }

    private void OnDisable()
    {
        if (button != null) button.onClick.RemoveListener(InvokeLevel);
    }

    public void InvokeLevel()
    {
        if (panel) panel.OnLevelBtnClicked(levelNo);
        
    }

    public void RefreshStars(com.VisionXR.ModelClasses.TrickShotsDataSO data)
    {
        if (!data) return;
        EnsureStarRefs();

        int total = (starCGs != null) ? starCGs.Length : 0;
        if (total == 0) return;

        int best = Mathf.Clamp(data.GetBestStars(levelNo), 0, total);
      
        if (best > 0)
        {
            for (int i = 0; i < total; i++)
            {
                var cg = starCGs[i];
                if (!cg) continue;

                bool earned = i < best;

                // Always force a deterministic visual state each refresh
                if (hideUnlitStars)
                {
                    cg.gameObject.SetActive(earned);
                    cg.alpha = earned ? litAlpha : 0f;     // set alpha even if we hide, for safety
                }
                else
                {
                    cg.gameObject.SetActive(true);
                    cg.alpha = earned ? litAlpha : dimAlpha;
                }

                // Optional: ensure parent CanvasGroups don't override
                cg.ignoreParentGroups = false; // set true if a parent CG keeps forcing alpha on you
            }
        }
    }

    private void EnsureStarRefs()
    {
        // IMPORTANT: only direct children, not GetComponentsInChildren (which might include the parent)
        if ((starCGs == null || starCGs.Length == 0) && starsRoot)
        {
            int childCount = starsRoot.childCount;
            var list = new System.Collections.Generic.List<CanvasGroup>(childCount);
            for (int i = 0; i < childCount; i++)
            {
                var child = starsRoot.GetChild(i);
                var cg = child.GetComponent<CanvasGroup>();
                if (!cg) cg = child.gameObject.AddComponent<CanvasGroup>();
                list.Add(cg);
            }
            starCGs = list.ToArray();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!button)
        {
            button = GetComponent<Button>();
            if (!button) button = GetComponentInChildren<Button>(true);
        }
        EnsureStarRefs();
    }
#endif
}
