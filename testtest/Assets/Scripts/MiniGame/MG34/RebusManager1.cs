using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RebusManager1 : MonoBehaviour
{
    [System.Serializable]
    public class RebusQuestion
    {
        public string rebusName;      // Просто для порядка в инспекторе
        public TMP_InputField inputField; // Поле ввода для этого ребуса
        public Image resultImage;     // Картинка-ответ, которая появится (если нужно)
        public Image backImage;
        public Image backImage2;
        public Image backImage3;
        public string correctText;    // Правильный ответ
    }

    public List<RebusQuestion> questions; // Список всех 3 ребусов
    public TextMeshProUGUI statusText;    // Общий текст результата

    void Start()
    {
        // При старте скрываем картинки-ответы, если они назначены
        foreach (var q in questions)
        {
            if (q.resultImage != null) q.resultImage.gameObject.SetActive(false);
        }
    }

    public void CheckAllAnswers()
    {
        int correctCount = 0;

        foreach (var q in questions)
        {
            // 1. Убираем пробелы по краям, делаем маленькими буквами
            // 2. .Replace(" ", "") — УДАЛЯЕТ ВСЕ ПРОБЕЛЫ ВНУТРИ (чтобы "два слова" стали как одно)
            string userText = q.inputField.text.Trim().ToLower().Replace(" ", "");
            string targetText = q.correctText.Trim().ToLower().Replace(" ", "");

            // Проверяем, что поле не пустое и тексты совпали без учета пробелов
            if (userText == targetText && targetText != "")
            {
                correctCount++;
                q.inputField.image.color = Color.green; // Зеленый — верно
                if (q.resultImage != null) { q.resultImage.gameObject.SetActive(true); q.backImage.gameObject.SetActive(false); q.backImage2.gameObject.SetActive(false); q.backImage3.gameObject.SetActive(false); }
            }
            else
            {
                q.inputField.image.color = Color.red; // Красный — ошибка
            }
        }

        if (correctCount == questions.Count) { 
            statusText.text="Все ребусы разгаданы!";
            statusText.color = Color.forestGreen;
        }
        else { 
            statusText.text= $"Верно: {correctCount} из {questions.Count}";
            statusText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        foreach (var q in questions)
        {
            q.inputField.text = "";
            q.inputField.image.color = Color.white;
            if (q.resultImage != null) { q.resultImage.gameObject.SetActive(false); q.backImage.gameObject.SetActive(true); q.backImage2.gameObject.SetActive(true); q.backImage3.gameObject.SetActive(true); }
            }
        statusText.text = "Разгадайте ребусы по истории ВКЛ";
        statusText.color = Color.black;
    }
}