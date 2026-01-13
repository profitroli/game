using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableCard3 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int currentSlotID = -1;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling(); // Выводит карточку на передний план при таскании
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        SlotID closestSlot = null;
        float closestDistance = 100f; // Радиус захвата в пикселях

        // Ищем ближайший свободный слот
        SlotID[] allSlots = FindObjectsByType<SlotID>(FindObjectsSortMode.None);
        foreach (var slot in allSlots)
        {
            float dist = Vector2.Distance(rectTransform.position, slot.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestSlot = slot;
            }
        }

        if (closestSlot != null)
        {
            // Прикрепляем к центру окошка
            rectTransform.position = closestSlot.transform.position;
            currentSlotID = closestSlot.id;
        }
        else
        {
            // Если не попали — возвращаемся на старт
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
        currentSlotID = -1;
    }
}