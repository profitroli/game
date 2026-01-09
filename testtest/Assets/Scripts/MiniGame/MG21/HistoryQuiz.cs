using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HistoryQuiz : MonoBehaviour
{
    [System.Serializable]
    public class HistoryItem
    {
        public string question;     // Текст события
        public bool isIndependence; // Правильный ответ: true - Независимость, false - Перестройка
    }

    public List<HistoryItem> questions; // Список всех событий
    public TextMeshProUGUI eventDisplay; // Ссылка на текст на карточке
    public TextMeshProUGUI resultDisplay; // Ссылка на статус (Верно/Ошибка)
    public GameObject finishPanel;       // Окно финиша (панель)
    public TextMeshProUGUI resultText;   // Итоговый текст на панели

    public float speed = 250f;
    private int currentIndex = 0;

    void Start()
    {
        ShowNextQuestion();
    }
    void FinishGame()
    {

        if (finishPanel != null)
        {
            finishPanel.SetActive(true); // Включаем окно финиша
            resultText.text = $"ФИНИШ! Хороший результат, продолжай изучать!";
        }

    }
    // Эту функцию вызываем при нажатии кнопок
    public void SelectAnswer(bool userChoice)
    {
        if (currentIndex >= questions.Count) return;

        // Проверяем: совпал ли выбор игрока с правдой в списке
        if (userChoice == questions[currentIndex].isIndependence)
        {
            resultDisplay.text = "ВЕРНО!";
            resultDisplay.color = Color.green;
            currentIndex++;
            Invoke("ShowNextQuestion", 0.5f); // Показать следующий вопрос через полсекунды
        }
        else
        {
            resultDisplay.text = "ОШИБКА! Попробуй еще раз.";
            resultDisplay.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void ShowNextQuestion()
    {
        if (currentIndex < questions.Count)
        {
            eventDisplay.text = questions[currentIndex].question;
            resultDisplay.text = "К какому периоду это относится?";
            resultDisplay.color = Color.black;
        }
        else
        {
            FinishGame();
        }
    }
}