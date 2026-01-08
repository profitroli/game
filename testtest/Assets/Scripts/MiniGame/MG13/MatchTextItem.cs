using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MatchTextItem : MonoBehaviour, IPointerClickHandler
{
    public string id;
    public bool isLeftColumn;
    [HideInInspector] public Vector3 originalPosition;

    private MatchTextManage manager;
    private TextMeshProUGUI textMesh;

    void Awake() => originalPosition = transform.localPosition;

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