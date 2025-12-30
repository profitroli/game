using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchTextManager : MonoBehaviour
{
    [System.Serializable]
    public class MatchEntry
    {
        public string pairID;              // ID для связи (например, "1915")
        public TextMeshProUGUI dateText;   // Текст даты (слева)
        public TextMeshProUGUI eventText;  // Текст события (справа)
        [HideInInspector] public string selectedEventID = "";
    }

    public List<MatchEntry> matchEntries;
    public TextMeshProUGUI statusText;
    public Button checkButton;

    private string lastSelectedID = "";
    private TextMeshProUGUI lastSelectedText;

    void Start()
    {
        ResetMatch();
    }

    // Вызывается при клике на текст даты (нужен компонент Button или EventTrigger на тексте)
    public void SelectDate(int index)
    {
        lastSelectedID = matchEntries[index].pairID;
        lastSelectedText = matchEntries[index].dateText;

        statusText.text = "Выбрано: " + lastSelectedText.text;
        statusText.color = Color.blue;
    }

    // Вызывается при клике на текст события
    public void SelectEvent(string eventID)
    {
        if (string.IsNullOrEmpty(lastSelectedID)) return;

        foreach (var entry in matchEntries)
        {
            if (entry.pairID == lastSelectedID)
            {
                entry.selectedEventID = eventID;
                entry.dateText.color = Color.purple; // Помечаем выбранное
                break;
            }
        }

        lastSelectedID = "";
        statusText.text = "Пара сопоставлена. Нажмите 'Проверить'.";
    }

    public void CheckAll()
    {
        int correctCount = 0;
        foreach (var entry in matchEntries)
        {
            if (entry.selectedEventID == entry.pairID)
            {
                correctCount++;
                entry.dateText.color = Color.green;
                entry.eventText.color = Color.green;
            }
            else
            {
                entry.dateText.color = Color.red;
                entry.eventText.color = Color.red;
            }
        }

        statusText.text = (correctCount == matchEntries.Count) ? "ОДОБРЕНО!" : "ПЕРЕДЕЛАЙТЕ!";
        statusText.color = (correctCount == matchEntries.Count) ? Color.green : Color.red;
    }

    public void ResetMatch()
    {
        foreach (var entry in matchEntries)
        {
            entry.selectedEventID = "";
            entry.dateText.color = Color.black;
            entry.eventText.color = Color.black;
        }
        statusText.text = "Соедините тексты дат и событий";
        statusText.color = Color.black;
        lastSelectedID = "";
    }
}