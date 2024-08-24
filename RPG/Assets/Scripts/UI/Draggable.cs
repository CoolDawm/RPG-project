using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public Transform originalParent;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
        rectTransform.SetParent(transform.root, true); // Перемещаем объект в корень Canvas
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / rectTransform.lossyScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        if (rectTransform.parent!=originalParent)
        {
           // rectTransform.SetParent(eventData.pointerEnter.transform, true); // Устанавливаем нового родителя
        }
        else
        {
            Debug.Log("God Dammit");
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.SetParent(originalParent, true); // Возвращаем объект к исходному родителю
        }
    }
}
