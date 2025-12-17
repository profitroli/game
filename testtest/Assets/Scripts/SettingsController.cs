using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    // Метод для перехода на сцену подготовки
    public void LoadMainMenu()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("MainMenu");
    }
}