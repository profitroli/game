using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoController : MonoBehaviour, IPointerClickHandler
{
    [Header("Настройки объектов")]
    public RawImage displayImage; // Ссылка на RawImage, где идет видео
    public VideoPlayer videoPlayer; // Ссылка на компонент Video Player

    void Start()
    {
        // В начале делаем RawImage невидимым
        if (displayImage != null)
        {
            displayImage.enabled = false;
        }
    }

    // Метод срабатывает при клике на объект, где висит скрипт
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        if (videoPlayer != null && displayImage != null)
        {
            // Делаем картинку видимой
            displayImage.enabled = true;

            // Запускаем воспроизведение
            videoPlayer.Play();

            Debug.Log("Видео запущено!");
        }
    }
}