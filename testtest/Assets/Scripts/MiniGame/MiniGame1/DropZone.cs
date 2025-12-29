using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public string zoneType; // Напиши тут тип зоны в инспекторе

    public void OnDrop(PointerEventData eventData)
    {
        // Когда отпускаем карточку над кругом:
        GameObject dropped = eventData.pointerDrag;
        DragItem item = dropped.GetComponent<DragItem>();
        item.parentAfterDrag = transform; // Круг становится новым "домом" для карточки
    }
}