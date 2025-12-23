using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesSceneController : MonoBehaviour
{
    [Header("Кнопки игр")]
    [SerializeField] private Button[] gameButtons;

    [Header("Навигационные кнопки")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button okrPreparationButton; // Новая кнопка для перехода к ОКР

    [Header("Описание игр")]
    [SerializeField]
    private string[] gameNames = {
        "пример",
        "пример",
        "пример",
        "пример",
        "пример"
    };

    [Header("Настройки сцен")]
    [SerializeField] private string mainMenuName = "MainMenu";
    [SerializeField] private string okrPreparationSceneName = "PreparationScene"; // Название сцены подготовки к ОКР

    private GameObject currentMessage; // Для отслеживания текущего сообщения

    void Start()
    {
        // Настраиваем кнопки игр
        for (int i = 0; i < Mathf.Min(gameButtons.Length, gameNames.Length); i++)
        {
            int index = i;
            gameButtons[i].onClick.AddListener(() => OnGameButtonClick(index));

            // Меняем текст на кнопке
            Text buttonText = gameButtons[i].GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = gameNames[i];
            }
        }

        // Кнопка назад возвращает в главное меню
        if (backButton != null)
        {
            backButton.onClick.AddListener(ReturnToMainMenu);
        }

        // Настройка кнопки перехода к подготовке ОКР
        if (okrPreparationButton != null)
        {
            // Добавляем обработчик клика
            okrPreparationButton.onClick.AddListener(GoToOKRPreparation);
        }
        else
        {
            Debug.LogWarning("Кнопка перехода к ОКР не назначена в инспекторе!");
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void ReturnToMainMenu()
    {
        // Прямая загрузка сцены без эффектов затемнения
        SceneManager.LoadScene(mainMenuName);
    }

    void GoToOKRPreparation()
    {
        Debug.Log($"Переход к подготовке ОКР: {okrPreparationSceneName}");

        // Проверяем, существует ли сцена с таким именем
        if (SceneExists(okrPreparationSceneName))
        {
            SceneManager.LoadScene(okrPreparationSceneName);
        }
        else
        {
            ShowErrorMessage($"Сцена '{okrPreparationSceneName}' не найдена!");
            Debug.LogError($"Сцена '{okrPreparationSceneName}' не найдена. Проверьте название в Build Settings.");
        }
    }

    void OnGameButtonClick(int gameIndex)
    {
        if (gameIndex >= 0 && gameIndex < gameNames.Length)
        {
            Debug.Log($"Выбрана игра: {gameNames[gameIndex]}");
            ShowGameMessage(gameNames[gameIndex]);
        }
    }

    void ShowGameMessage(string gameName)
    {
        // Удаляем предыдущее сообщение, если оно есть
        if (currentMessage != null)
        {
            Destroy(currentMessage);
        }

        // Создаем сообщение
        GameObject message = new GameObject("Message");
        currentMessage = message; // Сохраняем ссылку на текущее сообщение

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            message.transform.SetParent(canvas.transform);
        }

        // Позиционируем по центру
        RectTransform rect = message.AddComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(400, 200);

        // Добавляем фон
        Image background = message.AddComponent<Image>();
        background.color = new Color(0, 0, 0, 0.8f);

        // Добавляем текст
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(message.transform);

        Text text = textObj.AddComponent<Text>();
        text.text = $"Игра '{gameName}' в разработке!";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 24;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;

        // Растягиваем текст на всю панель
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // Удаляем через 2 секунды
        Destroy(message, 2f);
    }

    void ShowErrorMessage(string errorMessage)
    {
        // Удаляем предыдущее сообщение, если оно есть
        if (currentMessage != null)
        {
            Destroy(currentMessage);
        }

        // Создаем сообщение об ошибке
        GameObject message = new GameObject("ErrorMessage");
        currentMessage = message;

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            message.transform.SetParent(canvas.transform);
        }

        // Позиционируем по центру
        RectTransform rect = message.AddComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(500, 250);

        // Добавляем фон (красный для ошибки)
        Image background = message.AddComponent<Image>();
        background.color = new Color(0.8f, 0, 0, 0.9f);

        // Добавляем текст
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(message.transform);

        Text text = textObj.AddComponent<Text>();
        text.text = errorMessage;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 20;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = 24;

        // Растягиваем текст на всю панель
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 10);
        textRect.offsetMax = new Vector2(-10, -10);

        // Удаляем через 3 секунды
        Destroy(message, 3f);
    }

    bool SceneExists(string sceneName)
    {
        // Проверяем, существует ли сцена в Build Settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameInBuild == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    void OnDestroy()
    {
        // Очищаем ссылку при уничтожении объекта
        if (currentMessage != null)
        {
            Destroy(currentMessage);
        }
    }
}