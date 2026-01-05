using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Слайдер громкости")]
    public Slider volumeSlider;

    [Header("Текст (опционально)")]
    public Text volumeText;

    void Start()
    {
        // Устанавливаем начальное значение
        if (AudioManager.Instance != null)
        {
            volumeSlider.value = AudioManager.Instance.musicVolume;
        }

        // Обновляем текст
        UpdateText();

        // Подписываемся на изменение
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(value);
            UpdateText();
        }
    }

    void UpdateText()
    {
        if (volumeText != null)
        {
            int percent = Mathf.RoundToInt(volumeSlider.value * 100);
            volumeText.text = $"Громкость: {percent}%";
        }
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
    }
}