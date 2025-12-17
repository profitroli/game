using UnityEngine;
using UnityEngine.SceneManagement;

public class Page2NavigationManager : MonoBehaviour
{
    // Метод для перехода на сцену подготовки
    public void LoadPreparationScene1()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("PreparationScene");
    }

    // Метод для перехода на ContentPage2
    public void LoadContentPage3()
    {
        // Если ContentPage2 - это отдельная сцена
        SceneManager.LoadScene("ContentPage3");

        // Если ContentPage2 - это GameObject на текущей сцене
        // ActivateContentPage2();
    }

    // Альтернативный метод, если страницы - это GameObjects на одной сцене
    public void ActivateContentPage3()
    {
        // Найти ContentPage1 и ContentPage2
        GameObject contentPage2 = GameObject.Find("ContentPage2");
        GameObject contentPage3 = GameObject.Find("ContentPage3");

        if (contentPage2 != null && contentPage3 != null)
        {
            // Деактивировать текущую страницу
            contentPage2.SetActive(false);
            // Активировать новую страницу
            contentPage3.SetActive(true);
        }
        else
        {
            Debug.LogError("Не удалось найти ContentPage2 или ContentPage3");
        }
    }
}