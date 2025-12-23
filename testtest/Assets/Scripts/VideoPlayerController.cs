using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoPlayerController : MonoBehaviour
{
    [Header("Настройки видео")]
    public VideoClip videoClip;
    public RawImage videoDisplay;
    public Button playButton;

    [Header("Кнопка остановки")]
    public Button stopButton;
    public bool showStopButtonOnVideo = true;

    [Header("Слайдер прогресса")]
    public Slider progressSlider;
    public Text timeText;
    public bool allowSeeking = true;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private RenderTexture renderTexture;
    private bool isPaused = false;
    private bool isDraggingSlider = false;
    private double savedTimeWhenPaused = 0; // Сохраняем время при паузе

    void Start()
    {
        SetupVideoPlayer();
        SetupButtons();
        SetupProgressSlider();

        videoDisplay.gameObject.SetActive(false);

        if (stopButton != null)
            stopButton.gameObject.SetActive(false);

        if (progressSlider != null)
            progressSlider.gameObject.SetActive(false);
    }

    void SetupVideoPlayer()
    {
        renderTexture = new RenderTexture(1920, 1080, 24);
        videoDisplay.texture = renderTexture;

        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.clip = videoClip;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        audioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoFinished;

        // Подписываемся на событие подготовки видео
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void SetupButtons()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(PlayVideo);
        }

        if (stopButton != null)
        {
            stopButton.onClick.RemoveAllListeners();
            stopButton.onClick.AddListener(TogglePause);

            Text stopButtonText = stopButton.GetComponentInChildren<Text>();
            if (stopButtonText != null)
                stopButtonText.text = "⏸ Пауза";
        }
    }

    void SetupProgressSlider()
    {
        if (progressSlider != null)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = 1;
            progressSlider.value = 0;

            // Добавляем EventTrigger для обработки перетаскивания
            AddSliderEventTriggers();

            // Также добавляем слушатель для изменения значения
            progressSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        // Инициализируем текст времени
        if (timeText != null)
            timeText.text = "00:00 / --:--";
    }

    void AddSliderEventTriggers()
    {
        EventTrigger trigger = progressSlider.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = progressSlider.gameObject.AddComponent<EventTrigger>();
        }

        // Событие начала перетаскивания
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnSliderPointerDown(); });
        trigger.triggers.Add(pointerDownEntry);

        // Событие окончания перетаскивания
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnSliderPointerUp(); });
        trigger.triggers.Add(pointerUpEntry);

        // Событие отпускания кнопки мыши (на всякий случай)
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) => { OnSliderPointerUp(); });
        trigger.triggers.Add(endDragEntry);
    }

    // Вызывается при начале перетаскивания слайдера
    public void OnSliderPointerDown()
    {
        if (allowSeeking && videoPlayer != null && videoPlayer.length > 0)
        {
            isDraggingSlider = true;
            Debug.Log("Начало перетаскивания слайдера");
        }
    }

    // Вызывается при изменении значения слайдера (включая перетаскивание)
    void OnSliderValueChanged(float value)
    {
        if (allowSeeking && videoPlayer != null && videoPlayer.length > 0)
        {
            // Обновляем текст времени
            double targetTime = value * videoPlayer.length;

            // Если на паузе, используем сохраненное время для отображения
            if (isPaused)
            {
                UpdateTimeText((float)savedTimeWhenPaused, (float)videoPlayer.length);
            }
            else
            {
                UpdateTimeText((float)targetTime, (float)videoPlayer.length);
            }

            // Если пользователь перетаскивает слайдер, перематываем видео
            if (isDraggingSlider)
            {
                // Сохраняем состояние воспроизведения
                bool wasPlaying = videoPlayer.isPlaying && !isPaused;

                // Устанавливаем новое время
                videoPlayer.time = targetTime;
                if (isPaused)
                {
                    savedTimeWhenPaused = targetTime; // Обновляем сохраненное время
                }

                // Если видео было на паузе, не возобновляем воспроизведение
                if (!wasPlaying)
                {
                    videoPlayer.Pause();
                }

                Debug.Log($"Перемотка при перетаскивании: {targetTime:F2} секунд");
            }
        }
    }

    // Вызывается при отпускании слайдера
    public void OnSliderPointerUp()
    {
        if (allowSeeking && videoPlayer != null && videoPlayer.length > 0)
        {
            isDraggingSlider = false;

            // Если видео было на паузе до перетаскивания, оставляем на паузе
            if (isPaused && videoPlayer.isPrepared)
            {
                videoPlayer.Pause();
                // Обновляем сохраненное время после перетаскивания
                savedTimeWhenPaused = videoPlayer.time;
                UpdateTimeText((float)savedTimeWhenPaused, (float)videoPlayer.length);
            }

            Debug.Log("Окончание перетаскивания слайдера");
        }
    }

    // Альтернативный метод перемотки
    public void SeekToTime(float normalizedTime)
    {
        if (videoPlayer != null && videoPlayer.length > 0)
        {
            double targetTime = normalizedTime * videoPlayer.length;
            videoPlayer.time = targetTime;

            if (isPaused)
            {
                savedTimeWhenPaused = targetTime; // Обновляем сохраненное время
            }

            if (progressSlider != null)
                progressSlider.value = normalizedTime;

            Debug.Log($"Перемотка на: {targetTime:F2} секунд");
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer != null && videoClip != null)
        {
            if (isPaused)
            {
                ResumeVideo();
                return;
            }

            StartVideoPlayback();
        }
        else
        {
            Debug.LogError("Видео или VideoPlayer не настроены!");
        }
    }

    void StartVideoPlayback()
    {
        // Сначала подготавливаем видео, чтобы получить его длину
        if (!videoPlayer.isPrepared)
        {
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += OnVideoPreparedForPlayback;
        }
        else
        {
            ActuallyStartPlayback();
        }
    }

    void OnVideoPreparedForPlayback(VideoPlayer vp)
    {
        // Удаляем временный обработчик
        videoPlayer.prepareCompleted -= OnVideoPreparedForPlayback;
        ActuallyStartPlayback();
    }

    void ActuallyStartPlayback()
    {
        videoDisplay.gameObject.SetActive(true);

        if (playButton != null)
            playButton.gameObject.SetActive(false);

        if (stopButton != null)
        {
            stopButton.gameObject.SetActive(true);
            Text stopButtonText = stopButton.GetComponentInChildren<Text>();
            if (stopButtonText != null)
                stopButtonText.text = "⏸ Пауза";
        }

        if (progressSlider != null)
            progressSlider.gameObject.SetActive(true);

        // Обновляем текст времени с общей длиной видео
        UpdateTimeText(0f, (float)videoPlayer.length);

        videoPlayer.Play();
        isPaused = false;
        savedTimeWhenPaused = 0;
        Debug.Log("Воспроизведение видео: " + videoClip.name);
    }

    public void TogglePause()
    {
        if (videoPlayer == null) return;

        if (videoPlayer.isPlaying && !isPaused)
        {
            PauseVideo();
        }
        else if (isPaused)
        {
            ResumeVideo();
        }
    }

    void PauseVideo()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            isPaused = true;
            // Сохраняем текущее время при паузе
            savedTimeWhenPaused = videoPlayer.time;

            if (stopButton != null)
            {
                Text stopButtonText = stopButton.GetComponentInChildren<Text>();
                if (stopButtonText != null)
                    stopButtonText.text = "▶ Продолжить";
            }

            Debug.Log("Видео приостановлено. Сохраненное время: " + savedTimeWhenPaused);
        }
    }

    void ResumeVideo()
    {
        // Восстанавливаем время из сохраненного значения
        if (isPaused)
        {
            videoPlayer.time = savedTimeWhenPaused;
        }

        videoPlayer.Play();
        isPaused = false;

        if (stopButton != null)
        {
            Text stopButtonText = stopButton.GetComponentInChildren<Text>();
            if (stopButtonText != null)
                stopButtonText.text = "⏸ Пауза";
        }

        Debug.Log("Воспроизведение видео продолжено с времени: " + savedTimeWhenPaused);
    }

    public void StopVideo()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop();
        isPaused = false;
        savedTimeWhenPaused = 0;

        videoDisplay.gameObject.SetActive(false);

        if (playButton != null)
            playButton.gameObject.SetActive(true);

        if (stopButton != null)
            stopButton.gameObject.SetActive(false);

        if (progressSlider != null)
        {
            progressSlider.gameObject.SetActive(false);
            progressSlider.value = 0;
        }

        if (timeText != null)
            timeText.text = "00:00 / --:--";

        Debug.Log("Воспроизведение видео остановлено");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        videoDisplay.gameObject.SetActive(false);

        if (playButton != null)
            playButton.gameObject.SetActive(true);

        if (stopButton != null)
            stopButton.gameObject.SetActive(false);

        if (progressSlider != null)
            progressSlider.gameObject.SetActive(false);

        if (progressSlider != null)
            progressSlider.value = 1f;

        if (timeText != null && videoPlayer != null)
        {
            UpdateTimeText((float)videoPlayer.length, (float)videoPlayer.length);
        }

        isPaused = false;
        savedTimeWhenPaused = 0;
        Debug.Log("Видео завершено");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopVideo();
        }

        // Обновляем слайдер и время только если не перетаскиваем его
        if (!isDraggingSlider && videoPlayer != null && videoPlayer.isPrepared)
        {
            UpdateProgressSlider();
        }
    }

    void UpdateProgressSlider()
    {
        if (videoPlayer != null && progressSlider != null && videoPlayer.length > 0)
        {
            if (videoPlayer.isPlaying && videoPlayer.frameCount > 0)
            {
                float progress = (float)(videoPlayer.time / videoPlayer.length);
                progressSlider.value = progress;

                if (timeText != null)
                {
                    UpdateTimeText((float)videoPlayer.time, (float)videoPlayer.length);
                }
            }
            else if (isPaused)
            {
                // При паузе используем сохраненное время
                float progress = (float)(savedTimeWhenPaused / videoPlayer.length);
                progressSlider.value = progress;

                if (timeText != null)
                {
                    UpdateTimeText((float)savedTimeWhenPaused, (float)videoPlayer.length);
                }
            }
        }
    }

    void UpdateTimeText(float currentTime, float totalTime)
    {
        if (timeText != null)
        {
            string currentTimeStr = FormatTime(currentTime);
            string totalTimeStr = FormatTime(totalTime);
            timeText.text = $"{currentTimeStr} / {totalTimeStr}";
        }
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Если время не определено (NaN или отрицательное)
        if (float.IsNaN(timeInSeconds) || timeInSeconds < 0)
        {
            return "--:--";
        }

        return $"{minutes:00}:{seconds:00}";
    }

    // Новый метод для обработки подготовки видео
    void OnVideoPrepared(VideoPlayer vp)
    {
        // Этот метод вызывается, когда видео готово к воспроизведению
        Debug.Log("Видео подготовлено. Длина: " + vp.length + " секунд");

        // Можно обновить UI с общей длиной видео
        if (timeText != null && !videoPlayer.isPlaying)
        {
            timeText.text = "00:00 / " + FormatTime((float)videoPlayer.length);
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
            videoPlayer.prepareCompleted -= OnVideoPrepared;
        }

        if (renderTexture != null)
        {
            renderTexture.Release();
        }

        if (progressSlider != null)
        {
            progressSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}