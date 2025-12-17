using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LecturesController : MonoBehaviour
{
    [Header("Кнопки")]
    public Button backButton;
    public Button[] lectureButtons;
    public Button miniGamesButton; // Новая кнопка для мини-игр

    [Header("UI элементы")]
    public Text lectureTitle;
    public Text lectureContent;
    public GameObject contentPanel;

    void Start()
    {
        // Назначаем кнопку возврата
        if (backButton != null)
            backButton.onClick.AddListener(ReturnToMainMenu);

        // Назначаем кнопку мини-игр
        if (miniGamesButton != null)
            miniGamesButton.onClick.AddListener(GoToMiniGames);

        // Назначаем кнопки лекций
        for (int i = 0; i < lectureButtons.Length; i++)
        {
            int index = i; // Локальная копия для замыкания
            lectureButtons[i].onClick.AddListener(() => ShowLecture(index));
        }

        // Скрываем контент лекции при старте
        if (contentPanel != null)
            contentPanel.SetActive(false);
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture");
    }

    void ReturnToMainMenu()
    {
        // Возврат в главное меню без затемнения
        SceneManager.LoadScene("MainMenu");
    }

    void GoToMiniGames()
    {
        // Переход в сцену мини-игр
        // Убедитесь, что у вас есть сцена с именем "MiniGames" в Build Settings
        SceneManager.LoadScene("GamesScene");
    }

    void ShowLecture(int lectureIndex)
    {
        // Показываем контент лекции
        if (contentPanel != null)
            contentPanel.SetActive(true);

        // Здесь загружаем контент лекции по индексу
        // Пример:
        string[] lectureTitles = {
            "Введение в программирование",
            "Основы алгоритмов",
            "Структуры данных",
            "Объектно-ориентированное программирование"
        };

        string[] lectureContents = {
            "Программирование - это процесс создания компьютерных программ...",
            "Алгоритм - это последовательность шагов для решения задачи...",
            "Структуры данных позволяют эффективно организовывать и хранить данные...",
            "ООП - это парадигма программирования, основанная на концепции объектов..."
        };

        if (lectureIndex >= 0 && lectureIndex < lectureTitles.Length)
        {
            if (lectureTitle != null)
                lectureTitle.text = lectureTitles[lectureIndex];

            if (lectureContent != null)
                lectureContent.text = lectureContents[lectureIndex];
        }
    }
}