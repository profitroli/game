using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CrosswordHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 5)]
    public string question;             // Текст вопроса
    public TextMeshProUGUI displayText;  // Ссылка на сам текст
    public GameObject hintPanel;        // Ссылка на панель (фон)

    void Start()
    {
        // Прячем панель при старте игры
        if (hintPanel != null) hintPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hintPanel != null && displayText != null)
        {
            displayText.text = question;
            hintPanel.SetActive(true); // Показываем панель
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hintPanel != null)
        {
            displayText.text = "";
            hintPanel.SetActive(false); // Прячем панель
        }
    }
}