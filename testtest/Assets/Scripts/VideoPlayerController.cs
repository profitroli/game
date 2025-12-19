using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [Header("Настройки видео")]
    public VideoClip videoClip; // Перетащите сюда видеофайл из папки Assets
    public RawImage videoDisplay; // Перетащите сюда объект RawImage
    public Button playButton; // Перетащите сюда объект Button

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private RenderTexture renderTexture;

    void Start()
    {
        // Настраиваем компоненты
        SetupVideoPlayer();
        SetupButton();

        // Скрываем видео до нажатия кнопки
        videoDisplay.gameObject.SetActive(false);
    }

    void SetupVideoPlayer()
    {
        // Создаем RenderTexture для отображения видео
        renderTexture = new RenderTexture(1920, 1080, 24);
        videoDisplay.texture = renderTexture;

        // Добавляем компонент VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.clip = videoClip;

        // Настраиваем аудио
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        audioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // Настраиваем цикличность
        videoPlayer.isLooping = false;

        // Событие при завершении видео
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void SetupButton()
    {
        if (playButton != null)
        {
            // Убираем все предыдущие слушатели и добавляем свой
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(PlayVideo);
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer != null && videoClip != null)
        {
            // Показываем видео
            videoDisplay.gameObject.SetActive(true);

            // Скрываем кнопку
            if (playButton != null)
                playButton.gameObject.SetActive(false);

            // Воспроизводим видео
            videoPlayer.Play();
            Debug.Log("Воспроизведение видео: " + videoClip.name);
        }
        else
        {
            Debug.LogError("Видео или VideoPlayer не настроены!");
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Показываем кнопку снова после завершения видео
        if (playButton != null)
            playButton.gameObject.SetActive(true);

        Debug.Log("Видео завершено");
    }

    void OnDestroy()
    {
        // Очистка
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }

        if (renderTexture != null)
        {
            renderTexture.Release();
        }
    }
}