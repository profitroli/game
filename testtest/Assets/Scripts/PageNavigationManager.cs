using UnityEngine;
using UnityEngine.SceneManagement;

public class PageNavigationManager : MonoBehaviour
{
    // Метод для перехода на сцену подготовки
    public void LoadPreparationScene()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("PreparationScene");
    }

    // Метод для перехода на ContentPage2
    public void LoadContentPage2()
    {
        // Если ContentPage2 - это отдельная сцена
        SceneManager.LoadScene("ContentPage2");

        // Если ContentPage2 - это GameObject на текущей сцене
        // ActivateContentPage2();
    }

    // Альтернативный метод, если страницы - это GameObjects на одной сцене
    public void ActivateContentPage2()
    {
        // Найти ContentPage1 и ContentPage2
        GameObject contentPage1 = GameObject.Find("ContentPage1");
        GameObject contentPage2 = GameObject.Find("ContentPage2");

        if (contentPage1 != null && contentPage2 != null)
        {
            // Деактивировать текущую страницу
            contentPage1.SetActive(false);
            // Активировать новую страницу
            contentPage2.SetActive(true);
        }
        else
        {
            Debug.LogError("Не удалось найти ContentPage1 или ContentPage2");
        }
    }
}