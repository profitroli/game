using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PreparationController : MonoBehaviour
{
    [Header("ЭЛЕМЕНТЫ")]
    [SerializeField] private Button backButton;
    [SerializeField] private CanvasGroup contentGroup;

    [Header("НАСТРОЙКИ")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private float fadeTime = 0.8f;

    void Start()
    {
        // Назначаем обработчик кнопки
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClick);
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
        // Простой переход без затемнения
        SceneManager.LoadScene(mainMenuScene);
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
}  