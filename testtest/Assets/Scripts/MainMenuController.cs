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
    [SerializeField] private Button settingsButton;
    [SerializeField] private CanvasGroup menuCanvasGroup;

    [Header("Сцены для загрузки")]
    [SerializeField] private string lecturesSceneName = "LecturesScene";
    [SerializeField] private string miniGamesSceneName = "GamesScene";
    [SerializeField] private string preparationSceneName = "PreparationScene";
    [SerializeField] private string settingsSceneName = "SettingsScene";

    // Статические переменные для отслеживания состояния
    private static bool isFirstTime = true;
    private static bool shouldKeepBlackScreen = false;

    void Awake()
    {
        Debug.Log("MainMenu Awake - isFirstTime: " + isFirstTime);

        if (isFirstTime)
        {
            // Первый раз - оставляем черный экран для анимации
            isFirstTime = false;
            shouldKeepBlackScreen = true;
            Debug.Log("Это первый запуск, оставляем черный экран для анимации");
        }
        else
        {
            // Не первый раз - удаляем черный экран сразу
            shouldKeepBlackScreen = false;
            DestroyAllBlackScreensImmediately();
            Debug.Log("Не первый запуск, удаляем черный экран сразу");
        }
    }

    void Start()
    {
        Debug.Log("MainMenu Start - shouldKeepBlackScreen: " + shouldKeepBlackScreen);

        if (shouldKeepBlackScreen)
        {
            // Если нужно оставить черный экран для анимации
            StartCoroutine(ShowMenuWithFade());
            shouldKeepBlackScreen = false; // После анимации больше не нужно
        }
        else
        {
            // Если черный экран уже удален или не нужен
            ShowMenuInstantly();
        }

        InitializeButtons();
    }

    IEnumerator ShowMenuWithFade()
    {
        Debug.Log("Начинаем анимацию с черным экраном");

        // Ищем черный экран
        GameObject blackScreen = GameObject.Find("FadeImage");
        if (blackScreen == null)
        {
            blackScreen = GameObject.Find("BlackScreenCanvas");
        }

        // Ищем черный Image
        Image blackScreenImage = null;
        if (blackScreen != null)
        {
            blackScreenImage = blackScreen.GetComponent<Image>();
            if (blackScreenImage == null)
            {
                blackScreenImage = blackScreen.GetComponentInChildren<Image>();
            }
        }

        // Скрываем меню в начале
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }

        // Ждем небольшую паузу
        yield return new WaitForSeconds(0.5f);

        float timer = 0f;

        while (timer < fadeTime)
        {
            float progress = timer / fadeTime;

            // Плавно убираем черный экран
            if (blackScreenImage != null)
            {
                blackScreenImage.color = new Color(0, 0, 0, 1 - progress);
            }

            // Плавно показываем меню
            if (menuCanvasGroup != null)
            {
                menuCanvasGroup.alpha = progress;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Завершаем анимацию
        if (blackScreen != null)
        {
            Destroy(blackScreen);
            Debug.Log("Черный экран удален после анимации");
        }

        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;
        }

        Debug.Log("Анимация завершена, меню показано");
    }

    private void DestroyAllBlackScreensImmediately()
    {
        // Удаляем все возможные черные экраны
        GameObject[] blackScreenNames = {
            GameObject.Find("FadeImage"),
            GameObject.Find("BlackScreenCanvas"),
            GameObject.Find("BlackScreen")
        };

        foreach (GameObject obj in blackScreenNames)
        {
            if (obj != null)
            {
                Destroy(obj);
                Debug.Log("Удален черный экран: " + obj.name);
            }
        }

        // Ищем все черные изображения
        Image[] allImages = FindObjectsOfType<Image>(true);
        foreach (Image img in allImages)
        {
            if (img.color == Color.black && img.gameObject.name.Contains("Black"))
            {
                Destroy(img.gameObject);
                Debug.Log("Удалено черное изображение: " + img.name);
            }
        }
    }

    private void InitializeButtons()
    {
        // Назначаем обработчики для кнопок
        lecturesButton?.onClick.AddListener(LoadLecturesScene);
        miniGamesButton?.onClick.AddListener(LoadMiniGamesScene);
        preparationButton?.onClick.AddListener(LoadPreparationScene);
        exitButton?.onClick.AddListener(OnExitClick);
        settingsButton?.onClick.AddListener(LoadSettingsScene);
    }

    private void ShowMenuInstantly()
    {
        Debug.Log("Мгновенное отображение меню");

        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;
        }
    }

    // Статический метод для загрузки главного меню из других сцен
    public static void LoadMainMenuFromOtherScene()
    {
        Debug.Log("Загрузка главного меню из другой сцены");
        SceneManager.LoadScene("MainMenu");
    }

    // Методы загрузки сцен
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

    public void LoadSettingsScene()
    {
        LoadSceneWithValidation(settingsSceneName, "настроек");
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        lecturesButton?.onClick.RemoveAllListeners();
        miniGamesButton?.onClick.RemoveAllListeners();
        preparationButton?.onClick.RemoveAllListeners();
        exitButton?.onClick.RemoveAllListeners();
        settingsButton?.onClick.RemoveAllListeners();
    }
}