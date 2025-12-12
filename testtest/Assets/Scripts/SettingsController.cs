using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Элементы настроек")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Button backButton;
    [SerializeField] private Button saveButton;

    void Start()
    {
        // Загружаем сохраненные настройки
        LoadSettings();

        // Кнопка назад
        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });

        // Кнопка сохранения
        saveButton.onClick.AddListener(SaveSettings);
    }

    void LoadSettings()
    {
        // Загрузка сохраненных значений
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        // Настройки качества
        string[] qualityNames = QualitySettings.names;
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(qualityNames));
        qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
    }

    void SaveSettings()
    {
        // Сохраняем настройки
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("MusicEnabled", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("SFXEnabled", sfxToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);

        // Применяем настройки
        ApplySettings();

        Debug.Log("Настройки сохранены!");
    }

    void ApplySettings()
    {
        // Применение настроек
        AudioListener.volume = volumeSlider.value;
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }
}