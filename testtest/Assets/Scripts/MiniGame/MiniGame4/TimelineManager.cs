using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    [System.Serializable]
    public class TimelineEvent
    {
        public DraggableCard cardScript; // Тянем сюда саму карточку
        public int correctSlotID;       // Правильный ID слота (0, 1, 2...)
    }

    public List<TimelineEvent> events;
    public TextMeshProUGUI statusText;

    public void CheckResults()
    {
        int correctCount = 0;

        foreach (var ev in events)
        {
            if (ev.cardScript.currentSlotID == ev.correctSlotID)
            {
                correctCount++;
            }
        }

        if (correctCount == events.Count)
        {
            statusText.text = "Верно! Хронология соблюдена.";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "Ошибки в порядке! Попробуйте еще раз.";
            statusText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        if (statusText != null) statusText.text = "Расставьте события в хронологическом порядке";
        statusText.color= Color.black;

        foreach (var ev in events)
        {
            if (ev.cardScript != null)
            {
                ev.cardScript.ResetPosition();
            }
        }
    }
}