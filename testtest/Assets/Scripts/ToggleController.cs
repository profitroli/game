using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [Header("Ссылки")]
    public Toggle soundToggle;
    public Toggle musicToggle;
    public Toggle vibrationToggle;

    [Header("Настройки")]
    public AudioSource backgroundMusic;

    void Start()
    {
        // Загружаем сохраненные значения (пример)
        soundToggle.isOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        vibrationToggle.isOn = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;

        // Подписываемся на события
        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);

        // Применяем начальные настройки
        ApplySoundSettings();
        ApplyMusicSettings();
    }

    void OnSoundToggleChanged(bool isOn)
    {
        Debug.Log($"Звук: {(isOn ? "ВКЛ" : "ВЫКЛ")}");
        PlayerPrefs.SetInt("SoundEnabled", isOn ? 1 : 0);
        ApplySoundSettings();
    }

    void OnMusicToggleChanged(bool isOn)
    {
        Debug.Log($"Музыка: {(isOn ? "ВКЛ" : "ВЫКЛ")}");
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        ApplyMusicSettings();
    }

    void OnVibrationToggleChanged(bool isOn)
    {
        Debug.Log($"Вибрация: {(isOn ? "ВКЛ" : "ВЫКЛ")}");
        PlayerPrefs.SetInt("VibrationEnabled", isOn ? 1 : 0);

        // Пример для мобильных устройств
#if UNITY_ANDROID || UNITY_IOS
        if (isOn)
        {
            Handheld.Vibrate();
        }
#endif
    }

    void ApplySoundSettings()
    {
        // Применяем настройки звука ко всем звуковым эффектам
        AudioListener.volume = soundToggle.isOn ? 1f : 0f;
    }

    void ApplyMusicSettings()
    {
        if (backgroundMusic != null)
        {
            if (musicToggle.isOn && !backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
            else if (!musicToggle.isOn && backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
        }
    }

    void OnDestroy()
    {
        // Отписываемся от событий при уничтожении объекта
        soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
        musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
        vibrationToggle.onValueChanged.RemoveListener(OnVibrationToggleChanged);
    }
}