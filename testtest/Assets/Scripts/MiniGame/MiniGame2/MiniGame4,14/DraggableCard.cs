using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int currentSlotID = -1;    // ID слота, в котором сейчас лежит карта
    private Vector2 originalPosition; // Позиция для кнопки "Сброс"
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        // Запоминаем позицию, которая была в самом начале в редакторе
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; // Позволяет "видеть" слоты сквозь карту
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Ищем, попали ли мы в какой-нибудь слот
        SlotID slot = null;
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<SlotID>())
            {
                slot = result.gameObject.GetComponent<SlotID>();
                break;
            }
        }

        if (slot != null)
        {
            // "Магнитим" карту к центру слота
            rectTransform.position = slot.transform.position;
            currentSlotID = slot.id;
        }
        else
        {
            currentSlotID = -1; // Мы в пустоте
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
        currentSlotID = -1;
    }
}