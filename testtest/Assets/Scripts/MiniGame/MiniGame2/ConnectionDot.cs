using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionDot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public int id;
    public bool isLeft;
    private LineManager lineManager;

    void Start()
    {
        lineManager = FindObjectOfType<LineManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lineManager.StartDrawing(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Проверяем, над каким объектом сейчас находится мышь
        GameObject hovered = eventData.pointerEnter;

        if (hovered != null)
        {
            ConnectionDot endDot = hovered.GetComponent<ConnectionDot>();
            if (endDot != null)
            {
                lineManager.EndDrawing(endDot);
                return;
            }
        }

        // Если отпустили не над точкой — отмена
        lineManager.EndDrawing(null);
    }

    // Этот метод помогает Unity лучше определять попадание в точку
    public void OnPointerEnter(PointerEventData eventData) { }
}