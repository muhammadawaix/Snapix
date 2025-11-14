using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static public PuzzlePiece instance;
    [HideInInspector] public PuzzleManager manager;
    [HideInInspector] public int slotIndex;
    [HideInInspector] public int currentSlot;
    [HideInInspector] public bool isLocked = false;

    [HideInInspector] public RectTransform rect;
    CanvasGroup canvasGroup;
    Vector2 startPos;

    void Awake()
    {
        instance = this;
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
        GamePlay.instance.DragSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        
        rect.anchoredPosition += eventData.delta / (rect.GetComponentInParent<Canvas>()?.scaleFactor ?? 1f);
    }


    public void OnEndDrag(PointerEventData eventData)
{
    if (isLocked) return;
    canvasGroup.blocksRaycasts = true;

    
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
            
            int tmpSlot = nearest.currentSlot;
            nearest.currentSlot = this.currentSlot;
            this.currentSlot = tmpSlot;

            
            if (manager != null && manager.slots != null && manager.slots.Count > 0)
            {
                nearest.rect.anchoredPosition = manager.slots[nearest.currentSlot].anchoredPosition;
                rect.anchoredPosition = manager.slots[this.currentSlot].anchoredPosition;
            }
        }
        else
        {
            
            if (manager != null && manager.slots != null && manager.slots.Count > 0)
            {
                rect.anchoredPosition = manager.slots[this.currentSlot].anchoredPosition;
            }
            else
            {
                rect.anchoredPosition = startPos;
            }
        }

    
    Vector2 correctPos = PuzzleManager.instance.GetSlotPosition(slotIndex);
    float distToCorrect = Vector2.Distance(rect.anchoredPosition, correctPos);
    if (distToCorrect <= manager.snapThreshold)
    {
        isLocked = true;
        rect.anchoredPosition = correctPos;
    }
    else
    {
        isLocked = false;
    }


    if (PuzzleManager.instance != null)
        PuzzleManager.instance.CheckPuzzleComplete();
}

}