using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float fadeTime = 2f;

    [Header("Ссылки на элементы UI")]
    [SerializeField] private Button lecturesButton;
    [SerializeField] private Button miniGamesButton;
    [SerializeField] private Button preparationButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private CanvasGroup menuCanvasGroup;

    [Header("Сцены для загрузки")]
    [SerializeField] private string lecturesSceneName = "LecturesScene";
    [SerializeField] private string miniGamesSceneName = "GamesScene";
    [SerializeField] private string preparationSceneName = "PreparationScene";
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Добавили ссылку на главное меню

    private const string BLACK_SCREEN_NAME = "IntroBlackScreen";

    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    private void Start()
    {
        InitializeButtons();
        InitializeMenuVisibility();
    }

    private void InitializeButtons()
    {
        // Назначаем обработчики для кнопок
        lecturesButton?.onClick.AddListener(LoadLecturesScene);
        miniGamesButton?.onClick.AddListener(LoadMiniGamesScene);
        preparationButton?.onClick.AddListener(LoadPreparationScene);
        exitButton?.onClick.AddListener(OnExitClick);
    }

    private void InitializeMenuVisibility()
    {
        // Скрываем меню в начале
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }

        // Проверяем наличие черного экрана
        GameObject blackScreen = GameObject.Find(BLACK_SCREEN_NAME);

        if (blackScreen != null)
        {
            StartCoroutine(ShowMenuWithFade(blackScreen));
        }
        else
        {
            ShowMenuInstantly();
        }
    }

    private System.Collections.IEnumerator ShowMenuWithFade(GameObject blackScreen)
    {
        // Ждем небольшую паузу перед началом анимации
        yield return new WaitForSeconds(0.7f);

        Image blackScreenImage = blackScreen.GetComponent<Image>();
        float timer = 0f;

        // Плавная анимация появления меню и исчезновения черного экрана
        while (timer < fadeTime)
        {
            float progress = timer / fadeTime;

            // Управляем прозрачностью черного экрана
            if (blackScreenImage != null)
            {
                blackScreenImage.color = new Color(0, 0, 0, 1 - progress);
            }

            // Управляем прозрачностью меню
            if (menuCanvasGroup != null)
            {
                menuCanvasGroup.alpha = progress;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Завершаем анимацию
        FinishMenuShowAnimation(blackScreen);
    }

    private void FinishMenuShowAnimation(GameObject blackScreen)
    {
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
            menuCanvasGroup.blocksRaycasts = true;
        }
    }

    private void ShowMenuInstantly()
    {
        // Мгновенное отображение меню (если нет черного экрана)
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;
        }
    }

    // Методы загрузки сцен с проверкой существования
    private void LoadLecturesScene()
    {
        LoadSceneWithValidation(lecturesSceneName, "лекций");
    }

    private void LoadMiniGamesScene()
    {
        LoadSceneWithValidation(miniGamesSceneName, "мини-игр");
    }

    private void LoadPreparationScene()
    {
        LoadSceneWithValidation(preparationSceneName, "подготовки к ОКР");
    }

    private void LoadSceneWithValidation(string sceneName, string sceneDescription)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"Имя сцены {sceneDescription} не установлено!");
            ShowErrorMessage($"Сцена {sceneDescription} не настроена");
            return;
        }

        if (!IsSceneInBuildSettings(sceneName))
        {
            Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings!");
            ShowErrorMessage($"Сцена '{sceneName}' не найдена.\nДобавьте её в Build Settings.");
            return;
        }

        Debug.Log($"Загрузка сцены: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Проверка существования сцены в Build Settings
    private bool IsSceneInBuildSettings(string sceneName)
    {
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

    // Показать сообщение об ошибке
    private void ShowErrorMessage(string message)
    {
        GameObject errorObj = new GameObject("ErrorMessage");
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            errorObj.transform.SetParent(canvas.transform);
            errorObj.transform.SetAsLastSibling();
        }

        RectTransform rect = errorObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(400, 200);
        rect.anchoredPosition = Vector2.zero;

        Image bg = errorObj.AddComponent<Image>();
        bg.color = new Color(0.8f, 0.2f, 0.2f, 0.9f);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(errorObj.transform);

        Text text = textObj.AddComponent<Text>();
        text.text = message;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 18;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 10);
        textRect.offsetMax = new Vector2(-10, -10);

        Destroy(errorObj, 3f);
    }

    private void OnExitClick()
    {
        // Выход из приложения
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        // Отписываемся от событий для предотвращения утечек памяти
        lecturesButton?.onClick.RemoveAllListeners();
        miniGamesButton?.onClick.RemoveAllListeners();
        preparationButton?.onClick.RemoveAllListeners();
        exitButton?.onClick.RemoveAllListeners();
    }
}
