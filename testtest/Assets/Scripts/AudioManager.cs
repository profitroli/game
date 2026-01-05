using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Музыка")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;

    [Header("Громкость")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Header("Настройки запуска")]
    public string mainMenuSceneName = "MainMenu"; // Название сцены главного меню

    private const string VOLUME_KEY = "MusicVolume";
    private bool musicStarted = false;

    void Awake()
    {
        // Создаем единственный экземпляр
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Проверяем текущую сцену
        CheckCurrentScene();
    }

    void SetupAudio()
    {
        // Создаем источник звука
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false; // НЕ играть сразу!

        // Загружаем сохраненную громкость
        musicVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.7f);
        musicSource.volume = musicVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Проверяем, загрузилось ли главное меню
        if (scene.name == mainMenuSceneName && !musicStarted)
        {
            StartBackgroundMusic();
        }
    }

    void CheckCurrentScene()
    {
        // Проверяем текущую сцену при старте
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == mainMenuSceneName && !musicStarted)
        {
            StartBackgroundMusic();
        }
    }

    void StartBackgroundMusic()
    {
        if (backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.Play();
            musicStarted = true;
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;

        // Сохраняем
        PlayerPrefs.SetFloat(VOLUME_KEY, musicVolume);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}