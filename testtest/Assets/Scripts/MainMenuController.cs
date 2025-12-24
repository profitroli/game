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
    [SerializeField] private Button settingsButton; // Добавим кнопку настроек
    [SerializeField] private CanvasGroup menuCanvasGroup;

    [Header("Сцены для загрузки")]
    [SerializeField] private string lecturesSceneName = "LecturesScene";
    [SerializeField] private string miniGamesSceneName = "GamesScene";
    [SerializeField] private string preparationSceneName = "PreparationScene";
    [SerializeField] private string settingsSceneName = "SettingsScene"; // Сцена настроек
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Черный экран")]
    [SerializeField] private Image blackScreenImage; // Ссылка на Image черного экрана
    [SerializeField] private GameObject blackScreenObject; // Или ссылка на GameObject

    private const string BLACK_SCREEN_NAME = "IntroBlackScreen";

    // Статическая переменная для отслеживания источника перехода
    private static bool cameFromIntroVideo = false;
    private static bool firstTime = true;

    void Awake()
    {
        // Если это первый запуск, сбрасываем флаги
        if (firstTime)
        {
            cameFromIntroVideo = true;
            firstTime = false;
        }

        Debug.Log("MainMenuController: Awake - cameFromIntroVideo = " + cameFromIntroVideo);
    }

    public void LoadSettingsScene()
    {
        LoadSceneWithValidation(settingsSceneName, "настроек");
    }

    private void Start()
    {
        InitializeButtons();
        InitializeMenuVisibility();
        Debug.Log("MainMenuController: Start - cameFromIntroVideo = " + cameFromIntroVideo);
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

    private void InitializeMenuVisibility()
    {
        // Скрываем меню в начале
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }

        // Ищем черный экран разными способами
        GameObject blackScreen = null;

        // Способ 1: По имени
        blackScreen = GameObject.Find(BLACK_SCREEN_NAME);

        // Способ 2: По тегу (если есть)
        if (blackScreen == null)
        {
            GameObject[] blackScreens = GameObject.FindGameObjectsWithTag("BlackScreen");
            if (blackScreens.Length > 0)
            {
                blackScreen = blackScreens[0];
            }
        }

        // Способ 3: Через сериализованное поле
        if (blackScreen == null && blackScreenObject != null)
        {
            blackScreen = blackScreenObject;
        }

        // Способ 4: Ищем компонент Image с черным цветом
        if (blackScreen == null)
        {
            Image[] allImages = FindObjectsOfType<Image>(true); // true чтобы найти неактивные
            foreach (Image img in allImages)
            {
                if (img.color == Color.black && img.gameObject.name.Contains("Black"))
                {
                    blackScreen = img.gameObject;
                    break;
                }
            }
        }

        Debug.Log("MainMenuController: Найден черный экран? " + (blackScreen != null));
        Debug.Log("MainMenuController: Пришли из интро? " + cameFromIntroVideo);

        if (blackScreen != null)
        {
            if (cameFromIntroVideo)
            {
                // Если пришли из видео-заставки - показываем анимацию с черным экраном
                Debug.Log("MainMenuController: Запускаем анимацию с черным экраном");
                StartCoroutine(ShowMenuWithFade(blackScreen));
                cameFromIntroVideo = false; // Сбрасываем флаг
            }
            else
            {
                // Если вернулись из другой сцены - сразу удаляем черный экран
                Debug.Log("MainMenuController: Удаляем черный экран сразу");
                Destroy(blackScreen);
                ShowMenuInstantly();
            }
        }
        else
        {
            // Если черного экрана нет - показываем меню сразу
            Debug.Log("MainMenuController: Черный экран не найден, показываем меню сразу");
            ShowMenuInstantly();
        }
    }

    private IEnumerator ShowMenuWithFade(GameObject blackScreen)
    {
        Debug.Log("MainMenuController: Начинаем анимацию появления");

        // Ждем небольшую паузу перед началом анимации
        yield return new WaitForSeconds(0.7f);

        Image blackScreenImage = blackScreen.GetComponent<Image>();
        if (blackScreenImage == null)
        {
            // Если нет компонента Image, пробуем найти в дочерних объектах
            blackScreenImage = blackScreen.GetComponentInChildren<Image>();
        }

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
        Debug.Log("MainMenuController: Завершаем анимацию");

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
        Debug.Log("MainMenuController: Мгновенное отображение меню");

        // Мгновенное отображение меню (если нет черного экрана)
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
        Debug.Log("MainMenuController: Загрузка главного меню из другой сцены");
        cameFromIntroVideo = false; // Указываем, что мы НЕ из видео
        SceneManager.LoadScene("MainMenu");
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
        settingsButton?.onClick.RemoveAllListeners();
    }
}