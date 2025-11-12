using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public PuzzleManager manager;
    [HideInInspector] public int slotIndex;
    [HideInInspector] public bool isLocked = false;

    [HideInInspector] public RectTransform rect;
    CanvasGroup canvasGroup;
    Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        startPos = rect.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        // account for Canvas scaling automatically via eventData.delta (works for screen space)
        rect.anchoredPosition += eventData.delta / (rect.GetComponentInParent<Canvas>()?.scaleFactor ?? 1f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        canvasGroup.blocksRaycasts = true;

        // find nearest piece (not self)
        PuzzlePiece nearest = null;
        float minDist = Mathf.Infinity;
        foreach (var p in manager.pieces)
        {
            if (p == this) continue;
            float d = Vector2.Distance(rect.anchoredPosition, p.rect.anchoredPosition);
            if (d < minDist)
            {
                minDist = d;
                nearest = p;
            }
        }

        if (nearest != null && minDist <= manager.snapThreshold)
        {
            // swap positions
            Vector2 tmpPos = nearest.rect.anchoredPosition;
            nearest.rect.anchoredPosition = startPos;
            rect.anchoredPosition = tmpPos;

            // swap slotIndex values so each piece keeps its logical target
            int tmpIndex = nearest.slotIndex;
            nearest.slotIndex = this.slotIndex;
            this.slotIndex = tmpIndex;
        }
        else
        {
            // revert to original slot position
            rect.anchoredPosition = startPos;
        }
    }
}