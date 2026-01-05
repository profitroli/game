using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Добавь это для работы с Layout

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string type;
    [HideInInspector] public Transform parentAfterDrag;

    private Vector3 startPosition;
    private Transform startParent;
    private int startIndex; // ПОРЯДКОВЫЙ НОМЕР КАРТОЧКИ
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // ЗАПОМИНАЕМ ПРИ СТАРТЕ:
        startParent = transform.parent;
        startPosition = transform.localPosition;
        startIndex = transform.GetSiblingIndex(); // Запомнили, какой карточка была по счету (0, 1, 2...)
    }

    public void ResetCard()
    {
        // 1. Возвращаем в правильную панель
        transform.SetParent(startParent);

        // 2. ВОЗВРАЩАЕМ ПОРЯДКОВЫЙ НОМЕР (это самое главное для Layout Group)
        transform.SetSiblingIndex(startIndex);

        // 3. Сбрасываем позицию
        transform.localPosition = startPosition;

        parentAfterDrag = startParent;

        // 4. Принудительно просим Layout Group обновиться
        LayoutRebuilder.ForceRebuildLayoutImmediate(startParent as RectTransform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        canvasGroup.blocksRaycasts = true;

        // Обновляем Layout той зоны, куда бросили
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentAfterDrag as RectTransform);
    }
}