using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedRectTransform.SetParent(transform);
            draggedRectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
