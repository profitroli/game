using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;
    private AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // На всякий случай включаем звук программно
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // Принудительно 2D
    }

    void OnGUI()
    {
        // Проверяем событие клика мышки напрямую через систему событий окна
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log("OnGUI: Клик пойман!");
            PlayClick();
        }
    }

    public void PlayClick()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
            Debug.Log("Звук отправлен на воспроизведение: " + clickSound.name);
        }
        else
        {
            Debug.LogError("Ошибка: Не назначен звук или отсутствует AudioSource!");
        }
    }
}