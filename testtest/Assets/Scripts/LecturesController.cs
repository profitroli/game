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
    public void LoadLecture1()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 1");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture2()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 2");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture3()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 3");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture4()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 4");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture5()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 5");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture6()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 6");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture7()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 7");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture8()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 8");
    }

    // Метод для перехода на сцену подготовки
    public void LoadLecture9()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 9");
    }

    public void LoadLecture10()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 10");
    }

    public void LoadLecture11()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 11-12");
    }

    public void LoadLecture13()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 13");
    }

    public void LoadLecture14()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 14");
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