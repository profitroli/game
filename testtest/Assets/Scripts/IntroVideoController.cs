using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    [Header("НАСТРОЙКИ ВИДЕО")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;

    [Header("НАСТРОЙКИ АУДИО")]
    [SerializeField] private AudioSource audioSource; // AudioSource для воспроизведения звука видео
    [SerializeField] private float videoVolume = 1f; // Громкость видео

    [Header("ЭЛЕМЕНТЫ ИНТЕРФЕЙСА")]
    [SerializeField] private Button skipButton;
    [SerializeField] private Text loadingText;
    [SerializeField] private GameObject skipButtonObject;

    [Header("НАСТРОЙКИ ПЕРЕХОДА")]
    [SerializeField] private string nextSceneName = "MainMenu";
    [SerializeField] private float skipDelay = 1f; // Через сколько секунд можно пропустить
    [SerializeField] private bool allowSkip = true;

    [Header("ЭФФЕКТЫ")]
    [SerializeField] private Image fadeImage = null;
    [SerializeField] private float fadeDuration = 0.2f;

    private bool isVideoFinished = false;
    private float timer = 0f;
    private bool isVideoPrepared = false;

    void Start()
    {
        Debug.Log("Запуск интро сцены");

        // Инициализация компонентов
        InitializeComponents();

        // Настройка интерфейса
        SetupUI();

        // Настройка аудио
        SetupAudio();

        // Настройка событий видео
        SetupVideoEvents();

        // Начало воспроизведения видео
        PlayVideo();
    }

    void InitializeComponents()
    {
        // Автоматически находим компоненты если они не назначены
        if (videoPlayer == null)
            videoPlayer = FindObjectOfType<VideoPlayer>();

        if (videoDisplay == null)
            videoDisplay = FindObjectOfType<RawImage>();

        if (audioSource == null)
        {
            // Пытаемся найти существующий AudioSource на этом же GameObject
            audioSource = GetComponent<AudioSource>();

            // Если не нашли, создаем новый
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("Создан новый AudioSource");
            }
        }

        if (skipButton == null && skipButtonObject != null)
            skipButton = skipButtonObject.GetComponent<Button>();

        if (loadingText == null)
            loadingText = GetComponentInChildren<Text>();

        if (fadeImage == null)
        {
            // Создаем объект для затемнения если его нет
            CreateFadeImage();
        }
    }

    void SetupAudio()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource не найден!");
            return;
        }

        // Настраиваем AudioSource
        audioSource.volume = videoVolume;
        audioSource.spatialBlend = 0; // 2D звук
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Настраиваем VideoPlayer для использования этого AudioSource
        if (videoPlayer != null)
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.SetTargetAudioSource(0, audioSource); // Используем аудиодорожку 0
            videoPlayer.controlledAudioTrackCount = 1; // Используем одну аудиодорожку
            videoPlayer.EnableAudioTrack(0, true); // Включаем аудиодорожку
        }
    }

    void CreateFadeImage()
    {
        GameObject fadeObj = new GameObject("FadeImage");
        fadeObj.transform.SetParent(FindObjectOfType<Canvas>().transform);

        // Настраиваем RectTransform
        RectTransform rect = fadeObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Добавляем Image компонент
        fadeImage = fadeObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    void SetupUI()
    {
        // Настройка кнопки пропуска
        if (skipButton != null)
        {
            skipButton.onClick.RemoveAllListeners(); // Очищаем старые события
            skipButton.onClick.AddListener(SkipVideo);

            // Скрываем кнопку на время задержки
            if (skipButtonObject != null)
            {
                skipButtonObject.SetActive(false);
            }
        }
        else if (skipButtonObject != null)
        {
            // Если кнопка не назначена, но есть объект кнопки, пытаемся найти компонент Button
            skipButton = skipButtonObject.GetComponent<Button>();
            if (skipButton != null)
            {
                skipButton.onClick.RemoveAllListeners();
                skipButton.onClick.AddListener(SkipVideo);
            }
            else
            {
                // Если компонент Button отсутствует, добавляем его
                skipButton = skipButtonObject.AddComponent<Button>();
                skipButton.onClick.AddListener(SkipVideo);
            }
        }

        // Показываем текст загрузки
        if (loadingText != null)
        {
            loadingText.text = "ЗАГРУЗКА...";
            loadingText.gameObject.SetActive(true);
        }
    }

    void SetupVideoEvents()
    {
        if (videoPlayer != null)
        {
            // Событие: видео закончилось
            videoPlayer.loopPointReached += OnVideoFinished;

            // Событие: видео готово к воспроизведению
            videoPlayer.prepareCompleted += OnVideoPrepared;

            // Событие: видео началось
            videoPlayer.started += OnVideoStarted;

            // Событие: ошибка при загрузке видео
            videoPlayer.errorReceived += OnVideoError;
        }
        else
        {
            Debug.LogError("VideoPlayer не найден!");
        }
    }

    void Update()
    {
        // Таймер для активации кнопки пропуска
        if (skipButtonObject != null && !skipButtonObject.activeSelf && allowSkip && isVideoPrepared)
        {
            timer += Time.deltaTime;
            if (timer >= skipDelay)
            {
                skipButtonObject.SetActive(true);
                Debug.Log("Кнопка пропуска активирована");
            }
        }

        // Пропуск по любой клавише (после задержки)
        if (Input.anyKeyDown && allowSkip && timer >= skipDelay && !isVideoFinished && isVideoPrepared)
        {
            Debug.Log("Пропуск по клавише");
            SkipVideo();
        }
    }

    void PlayVideo()
    {
        if (videoPlayer != null)
        {
            StartCoroutine(PrepareAndPlayVideo());
        }
        else
        {
            Debug.LogError("Не могу воспроизвести видео: VideoPlayer не найден!");
            LoadMainMenu(); // Если видео нет, сразу грузим меню
        }
    }

    System.Collections.IEnumerator PrepareAndPlayVideo()
    {
        Debug.Log("Подготовка видео к воспроизведению...");

        if (loadingText != null)
            loadingText.text = "ПОДГОТОВКА ВИДЕО...";

        videoPlayer.Prepare();

        // Ждем пока видео подготовится
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Видео подготовлено, начинаю воспроизведение");

        if (loadingText != null)
            loadingText.gameObject.SetActive(false);

        // Активируем RawImage для отображения видео
        if (videoDisplay != null)
        {
            videoDisplay.texture = videoPlayer.texture;
            videoDisplay.gameObject.SetActive(true);
        }

        isVideoPrepared = true;

        // Запускаем видео и звук
        videoPlayer.Play();

        // Проверяем, что AudioSource начал воспроизводиться
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Видео готово к воспроизведению");

        if (loadingText != null)
        {
            loadingText.text = "";
            loadingText.gameObject.SetActive(false);
        }

        isVideoPrepared = true;
    }

    void OnVideoStarted(VideoPlayer vp)
    {
        Debug.Log("Видео началось");
    }

    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError($"Ошибка видео: {message}");

        // Если есть ошибка с видео, продолжаем без него
        if (loadingText != null)
        {
            loadingText.text = "ОШИБКА ВИДЕО. ПЕРЕХОДИМ В ГЛАВНОЕ МЕНЮ...";
        }

        StartCoroutine(FadeAndLoadScene());
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Видео завершено!");
        isVideoFinished = true;

        // Останавливаем аудио
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Запускаем переход в главное меню
        LoadMainMenu();
    }

    public void SkipVideo()
    {
        if (!isVideoFinished && allowSkip)
        {
            Debug.Log("Пользователь пропустил видео");
            isVideoFinished = true;

            // Останавливаем видео
            if (videoPlayer != null && videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
            }

            // Останавливаем аудио
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Загружаем главное меню
            LoadMainMenu();
        }
    }

    void LoadMainMenu()
    {
        Debug.Log("Загрузка главного меню...");
        StartCoroutine(FadeAndLoadScene());
    }

    System.Collections.IEnumerator FadeAndLoadScene()
    {
        Debug.Log("Начало плавного перехода...");

        // Создаем объект затемнения если его нет
        if (fadeImage == null)
        {
            CreateFadeImage();
        }

        // Плавное затемнение (Fade Out)
        float elapsedTime = 0f;

        // Плавно уменьшаем громкость видео при переходе
        float initialVolume = audioSource != null ? audioSource.volume : 0f;

        while (elapsedTime < fadeDuration)
        {
            if (fadeImage != null)
            {
                float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
            }

            // Плавно уменьшаем громкость звука
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Lerp(initialVolume, 0, elapsedTime / fadeDuration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (fadeImage != null)
        {
            fadeImage.color = Color.black;
        }

        // Полностью выключаем звук
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.Stop();
        }

        Debug.Log("Загружаю сцену: " + nextSceneName);

        // Загрузка главного меню
        SceneManager.LoadScene(nextSceneName);
    }

    // Метод для настройки громкости (можно вызвать из других скриптов)
    public void SetVideoVolume(float volume)
    {
        videoVolume = Mathf.Clamp01(volume);

        if (audioSource != null)
        {
            audioSource.volume = videoVolume;
        }
    }

    // Метод для получения текущей громкости
    public float GetVideoVolume()
    {
        return videoVolume;
    }

    void OnDestroy()
    {
        // Очищаем события чтобы не было утечек памяти
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            videoPlayer.started -= OnVideoStarted;
            videoPlayer.errorReceived -= OnVideoError;
        }

        // Очищаем события кнопки
        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(SkipVideo);
        }

        Debug.Log("IntroVideoController уничтожен");
    }
}