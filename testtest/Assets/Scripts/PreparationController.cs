using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PreparationController : MonoBehaviour
{
    [Header("ЭЛЕМЕНТЫ")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button lecturesButton;
    [SerializeField] private Button contentPage1Button; // Кнопка 1
    [SerializeField] private Button contentPage2Button; // Кнопка 2
    [SerializeField] private Button contentPage3Button; // Кнопка 3
    [SerializeField] private CanvasGroup contentGroup;

    [Header("НАСТРОЙКИ СЦЕН")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string lecturesScene = "LecturesScene";
    [SerializeField] private string contentPage1Scene = "ContentPage1"; // Сцена 1
    [SerializeField] private string contentPage2Scene = "ContentPage2"; // Сцена 2
    [SerializeField] private string contentPage3Scene = "ContentPage3"; // Сцена 3
    [SerializeField] private float fadeTime = 0.8f;

    void Start()
    {
        // Назначаем обработчики кнопок
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClick);
        }

        if (lecturesButton != null)
        {
            lecturesButton.onClick.AddListener(OnLecturesButtonClick);
        }
        else
        {
            Debug.LogWarning("Кнопка перехода к лекциям не назначена в инспекторе!");
        }

        // Обработчики для кнопок страниц контента
        if (contentPage1Button != null)
        {
            contentPage1Button.onClick.AddListener(() => OnContentPageButtonClick(contentPage1Scene, "Страница 1"));
        }
        else
        {
            Debug.LogWarning("Кнопка ContentPage1 не назначена в инспекторе!");
        }

        if (contentPage2Button != null)
        {
            contentPage2Button.onClick.AddListener(() => OnContentPageButtonClick(contentPage2Scene, "Страница 2"));
        }
        else
        {
            Debug.LogWarning("Кнопка ContentPage2 не назначена в инспекторе!");
        }

        if (contentPage3Button != null)
        {
            contentPage3Button.onClick.AddListener(() => OnContentPageButtonClick(contentPage3Scene, "Страница 3"));
        }
        else
        {
            Debug.LogWarning("Кнопка ContentPage3 не назначена в инспекторе!");
        }

        // Эффект появления контента (только при входе в сцену)
        if (contentGroup != null)
        {
            contentGroup.alpha = 0;
            StartCoroutine(FadeInContent());
        }
    }

    void OnBackButtonClick()
    {
        Debug.Log("Возврат в главное меню");
        SceneManager.LoadScene(mainMenuScene);
    }

    void OnLecturesButtonClick()
    {
        Debug.Log("Переход к лекциям");
        SceneManager.LoadScene(lecturesScene);
    }

    void OnContentPageButtonClick(string sceneName, string pageName)
    {
        Debug.Log($"Переход на {pageName}");
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeInContent()
    {
        yield return new WaitForSeconds(0.3f);

        float timer = 0f;
        while (timer < fadeTime)
        {
            contentGroup.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        contentGroup.alpha = 1;
    }

    // Опционально: метод с плавным переходом
    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        if (contentGroup != null)
        {
            float timer = 0f;
            while (timer < fadeTime)
            {
                contentGroup.alpha = Mathf.Lerp(1, 0, timer / fadeTime);
                timer += Time.deltaTime;
                yield return null;
            }
            contentGroup.alpha = 0;
        }

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }
}