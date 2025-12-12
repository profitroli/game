using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SimpleMenuController : MonoBehaviour
{
    [Header("Кнопки")]
    public Button lecturesButton;
    public Button miniGamesButton;
    public Button preparationButton;
    public Button exitButton;

    [Header("Элементы")]
    public Image backgroundPanel; // Ваш фон меню
    public CanvasGroup menuCanvasGroup;

    [Header("Настройки")]
    public float fadeTime = 2f;

    void Start()
    {
        // Назначаем кнопки
        if (lecturesButton != null) lecturesButton.onClick.AddListener(OnLecturesClick);
        if (miniGamesButton != null) miniGamesButton.onClick.AddListener(OnMiniGamesClick);
        if (preparationButton != null) preparationButton.onClick.AddListener(OnPreparationClick);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitClick);

        // Сначала все скрыто
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = false;
        }

        // Находим черный экран от интро
        GameObject blackScreen = GameObject.Find("IntroBlackScreen");

        if (blackScreen != null)
        {
            // Черный экран есть - плавно убираем его и показываем меню
            StartCoroutine(ShowMenuFromBlack(blackScreen));
        }
        else
        {
            // Черного экрана нет - просто показываем меню
            if (menuCanvasGroup != null)
            {
                menuCanvasGroup.alpha = 1;
                menuCanvasGroup.interactable = true;
            }
        }
    }

    IEnumerator ShowMenuFromBlack(GameObject blackScreen)
    {
        // Ждем немного
        yield return new WaitForSeconds(0.7f);

        Image blackImage = blackScreen.GetComponent<Image>();

        float timer = 0f;

        // Плавно убираем черный экран и показываем меню
        while (timer < fadeTime)
        {
            float progress = timer / fadeTime;

            // Убираем черный экран
            if (blackImage != null)
            {
                float alpha = Mathf.Lerp(1, 0, progress);
                blackImage.color = new Color(0, 0, 0, alpha);
            }

            // Показываем меню
            if (menuCanvasGroup != null)
            {
                menuCanvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Удаляем черный экран
        if (blackScreen != null)
        {
            Destroy(blackScreen);
        }

        // Активируем меню
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.interactable = true;
        }
    }

    void OnLecturesClick()
    {
        // Загружаем сцену с лекциями без затемнения
        SceneManager.LoadScene("LecturesScene");
    }

    void OnMiniGamesClick()
    {
        // Загружаем сцену с мини-играми без затемнения
        SceneManager.LoadScene("GamesScene");
    }

    void OnPreparationClick()
    {
        // Загружаем сцену подготовки без затемнения
        SceneManager.LoadScene("PreparationScene");
    }

    void OnExitClick()
    {
        // Простой выход без затемнения
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void ShowMessage(string message, float duration)
    {
        GameObject messageObj = new GameObject("Message");
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            messageObj.transform.SetParent(canvas.transform);
        }

        RectTransform rect = messageObj.AddComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(500, 150);

        Image bg = messageObj.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(messageObj.transform);

        Text text = textObj.AddComponent<Text>();
        text.text = message;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 24;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Destroy(messageObj, duration);
    }
}