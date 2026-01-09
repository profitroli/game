using UnityEngine;
using UnityEngine.EventSystems;

public class StepSlot : MonoBehaviour, IDropHandler
{
    public string slotID; // Порядок ступеньки (от 1 до 6)

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // Приклеиваем плашку к центру слота
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;
        }
    }
}