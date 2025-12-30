using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchTextManage : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    private MatchTextItem selectedDate;

    // Список всех пар для проверки
    private List<MatchTextItem> allDates = new List<MatchTextItem>();
    private List<MatchTextItem> allEvents = new List<MatchTextItem>();
    private Dictionary<MatchTextItem, MatchTextItem> currentMatches = new Dictionary<MatchTextItem, MatchTextItem>();

    public void SelectDate(MatchTextItem dateItem)
    {
        if (selectedDate != null) selectedDate.SetColor(Color.black);
        selectedDate = dateItem;
        selectedDate.SetColor(Color.blue);
        statusText.text = "Выбрано: " + dateItem.GetComponent<TextMeshProUGUI>().text;
    }

    public void SelectEvent(MatchTextItem eventItem)
    {
        if (selectedDate == null) return;

        // Если это событие уже было занято, очищаем старую связь
        MatchTextItem keyToRemove = null;
        foreach (var pair in currentMatches)
            if (pair.Value == eventItem) keyToRemove = pair.Key;
        if (keyToRemove != null) currentMatches.Remove(keyToRemove);

        currentMatches[selectedDate] = eventItem;
        selectedDate.SetColor(Color.purple); // Помечаем, что дата соединена
        eventItem.SetColor(Color.purple);    // Помечаем событие

        selectedDate = null;
        statusText.text = "Пара сопоставлена";
    }

    public void CheckAll()
    {
        int correctCount = 0;
        int total = FindObjectsByType<MatchTextItem>(FindObjectsSortMode.None).Length / 2;

        foreach (var pair in currentMatches)
        {
            if (pair.Key.id == pair.Value.id)
            {
                pair.Key.SetColor(Color.green);
                pair.Value.SetColor(Color.green);
                correctCount++;
            }
            else
            {
                pair.Key.SetColor(Color.red);
                pair.Value.SetColor(Color.red);
            }
        }

        statusText.text = (correctCount == total) ? "МОЛОДЕЦ! Хорошо разобрался в теме" : "ПЕРЕДЕЛАЙТЕ! Есть ошибки";
        statusText.color = (correctCount == total) ? Color.green : Color.red;
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        currentMatches.Clear();
        selectedDate = null;
        var allItems = FindObjectsByType<MatchTextItem>(FindObjectsSortMode.None);
        foreach (var item in allItems) item.SetColor(Color.black);
        statusText.text = "Соедините даты и события";
        statusText.color = Color.black;
    }
}