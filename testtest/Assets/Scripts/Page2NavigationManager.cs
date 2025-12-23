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

    public void LoadTest1()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test1");
    }

    public void LoadTest2()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test2");
    }

    public void LoadTest3()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test3");
    }

    public void LoadTest4()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test4");
    }

    public void LoadTest5()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test5");
    }

    public void LoadTest6()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test6");
    }

    public void LoadTest7()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test7");
    }

    public void LoadTest8()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test8");
    }

    public void LoadTest9()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test9");
    }

    public void LoadTest10()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test10");
    }

    public void LoadTest11()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test11");
    }

    public void LoadTest12()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test12");
    }

    public void LoadTest13()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test13");
    }

    public void LoadTest14()
    {
        // Убедитесь, что сцена "PreparationScene" добавлена в Build Settings
        SceneManager.LoadScene("Test14");
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