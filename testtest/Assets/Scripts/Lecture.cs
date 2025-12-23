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

    public void LoadLecture2()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 2");
    }

    public void LoadLecture3()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 3");
    }


    public void LoadLecture4()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 4");
    }

    public void LoadLecture5()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 5");
    }

    public void LoadLecture6()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 6");
    }

    public void LoadLecture7()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 7");
    }

    public void LoadLecture8()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 8");
    }

    public void LoadLecture9()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 9");
    }

    public void LoadLecture10()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 10");
    }

    public void LoadLecture11()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 11-12");
    }

    public void LoadLecture13()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 13");
    }

    public void LoadLecture14()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 14");
    }

    public void LoadLecture1()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Lecture 1");
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