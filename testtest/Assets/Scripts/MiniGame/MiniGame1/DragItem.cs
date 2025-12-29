using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string type;
    [HideInInspector] public Transform parentAfterDrag;

    // Новые переменные для сброса
    private Vector3 startPosition;
    private Transform startParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // Запоминаем начальную точку при запуске игры
        startPosition = transform.localPosition;
        startParent = transform.parent;
    }

    // Эта функция вернет карточку на базу
    public void ResetCard()
    {
        transform.SetParent(startParent);
        transform.localPosition = startPosition;
        parentAfterDrag = startParent;
    }

    // ... (остальные методы OnBeginDrag, OnDrag, OnEndDrag остаются прежними)
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
    }
}