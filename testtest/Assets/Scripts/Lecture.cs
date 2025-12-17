using UnityEngine;
using UnityEngine.SceneManagement;

public class Lecture : MonoBehaviour
{
    // Метод для перехода на сцену подготовки
    public void LoadLecturesScene()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("LecturesScene");
    }

    /*// Альтернативный метод, если страницы - это GameObjects на одной сцене
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
    }*/
}