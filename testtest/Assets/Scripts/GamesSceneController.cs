using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesSceneController : MonoBehaviour
{
    [Header("Кнопки игр")]
    [SerializeField] private Button[] gameButtons;

    [Header("Кнопка назад")]
    [SerializeField] private Button backButton;

    [Header("Описание игр")]
    [SerializeField]
    private string[] gameNames = {
        "пример",
        "пример",
        "пример",
        "пример",
        "пример"
    };

    [Header("Настройки")]
    [SerializeField] private string mainMenuSceneName = "MenuScene"; // Убедитесь, что название совпадает с вашей сценой меню

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

        // Кнопка назад возвращает в главное меню БЕЗ затемнения
        if (backButton != null)
        {
            backButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    void ReturnToMainMenu()
    {
        // Прямая загрузка сцены без эффектов затемнения
        // Убедитесь, что название сцены совпадает с вашей сценой меню
        SceneManager.LoadScene(mainMenuSceneName);
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

    void OnDestroy()
    {
        // Очищаем ссылку при уничтожении объекта
        if (currentMessage != null)
        {
            Destroy(currentMessage);
        }
    }
}