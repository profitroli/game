using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiDateManager : MonoBehaviour
{
    [System.Serializable]
    public class DateEntry
    {
        public string eventName;      // Название для инспектора (например, "Восстание Калиновского")
        public TMP_InputField inputField; // Поле, куда игрок пишет год
        public string correctYear;    // Правильный год (например, "1863")
        public Image iconImage;       // Картинка события (чтобы менять её цвет при проверке)
    }

    public List<DateEntry> dateEntries;
    public TextMeshProUGUI statusText;

    public void CheckAllDates()
    {
        int correctCount = 0;

        foreach (var entry in dateEntries)
        {
            // Очищаем ввод от лишних пробелов и букв (оставляем только цифры)
            string userYear = System.Text.RegularExpressions.Regex.Replace(entry.inputField.text, @"\D", "");

            if (userYear == entry.correctYear)
            {
                correctCount++;
                // Подсвечиваем поле зеленым
                entry.inputField.image.color = Color.green;
            }
            else
            {
                // Подсвечиваем поле красным
                entry.inputField.image.color = Color.red;
            }
        }

        // Общий результат внизу экрана
        if (correctCount == dateEntries.Count)
        {
            statusText.text = "Отлично! Все даты указаны верно.";
            statusText.color = Color.forestGreen;
        }
        else
        {
            statusText.text = $"Верно: {correctCount} из {dateEntries.Count}. Проверьте ошибки!";
            statusText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetDates()
    {
        foreach (var entry in dateEntries)
        {
            entry.inputField.text = "";
            entry.inputField.image.color = Color.white;
        }
        statusText.text = "Введите даты событий";
        statusText.color = Color.black;
    }
}