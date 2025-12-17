using UnityEngine;
using UnityEngine.SceneManagement;

public class Page3NavigationManager : MonoBehaviour
{
    // Метод для перехода на сцену подготовки
    public void LoadPreparationScene2()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("PreparationScene");
    }

    // Метод для перехода на ContentPage2
    public void LoadContentPage1()
    {
        // Если ContentPage2 - это отдельная сцена
        SceneManager.LoadScene("ContentPage1");

        // Если ContentPage2 - это GameObject на текущей сцене
        // ActivateContentPage2();
    }

    // Альтернативный метод, если страницы - это GameObjects на одной сцене
    public void ActivateContentPage1()
    {
        // Найти ContentPage1 и ContentPage2
        GameObject contentPage3 = GameObject.Find("ContentPage3");
        GameObject contentPage1 = GameObject.Find("ContentPage1");

        if (contentPage3 != null && contentPage1 != null)
        {
            // Деактивировать текущую страницу
            contentPage3.SetActive(false);
            // Активировать новую страницу
            contentPage1.SetActive(true);
        }
        else
        {
            Debug.LogError("Не удалось найти ContentPage2 или ContentPage3");
        }
    }
}