using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MatchTextItem : MonoBehaviour, IPointerClickHandler
{
    public string id; // Например, "1915"
    public bool isLeftColumn; // true для дат, false для событий
    private MatchTextManage manager;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        manager = FindFirstObjectByType<MatchTextManage>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLeftColumn) manager.SelectDate(this);
        else manager.SelectEvent(this);
    }

    public void SetColor(Color color) => textMesh.color = color;
}