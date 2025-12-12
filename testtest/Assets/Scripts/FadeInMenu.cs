using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInMenu : MonoBehaviour
{
    [Header("ЭЛЕМЕНТЫ МЕНЮ")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private Image blackScreen;

    [Header("НАСТРОЙКИ ПОЯВЛЕНИЯ")]
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private bool fadeBackground = true;

    void Start()
    {
        // Если не назначены, ищем автоматически
        if (menuCanvasGroup == null)
        {
            menuCanvasGroup = GetComponent<CanvasGroup>();
            if (menuCanvasGroup == null)
            {
                GameObject canvasObj = GameObject.Find("Canvas");
                if (canvasObj != null)
                {
                    menuCanvasGroup = canvasObj.GetComponent<CanvasGroup>();
                    if (menuCanvasGroup == null)
                    {
                        menuCanvasGroup = canvasObj.AddComponent<CanvasGroup>();
                    }
                }
            }
        }

        // Ищем черный экран
        if (blackScreen == null)
        {
            GameObject blackScreenObj = GameObject.Find("BlackScreen");
            if (blackScreenObj != null)
            {
                blackScreen = blackScreenObj.GetComponent<Image>();
            }
        }

        // Начинаем плавное появление
        StartCoroutine(FadeInSequence());
    }

    IEnumerator FadeInSequence()
    {
        // Инициализация: всё невидимо
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }

        // Создаем черный экран если его нет
        if (blackScreen == null && fadeBackground)
        {
            blackScreen = CreateBlackScreen();
        }

        if (blackScreen != null)
        {
            blackScreen.color = Color.black;
            blackScreen.gameObject.SetActive(true);
        }

        // Небольшая задержка перед началом
        yield return new WaitForSeconds(startDelay);

        // Плавное появление меню
        if (menuCanvasGroup != null)
        {
            float timer = 0f;
            while (timer < fadeInDuration)
            {
                float alpha = Mathf.Lerp(0, 1, timer / fadeInDuration);
                menuCanvasGroup.alpha = alpha;
                timer += Time.deltaTime;
                yield return null;
            }

            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;

            Debug.Log("Меню полностью появилось");
        }

        // Убираем черный экран
        if (blackScreen != null && fadeBackground)
        {
            StartCoroutine(FadeOutBlackScreen());
        }
    }

    IEnumerator FadeOutBlackScreen()
    {
        yield return new WaitForSeconds(0.5f);

        float timer = 0f;
        float fadeOutTime = 0.5f;
        Color startColor = blackScreen.color;
        Color endColor = new Color(0, 0, 0, 0);

        while (timer < fadeOutTime)
        {
            blackScreen.color = Color.Lerp(startColor, endColor, timer / fadeOutTime);
            timer += Time.deltaTime;
            yield return null;
        }

        blackScreen.color = endColor;
        blackScreen.gameObject.SetActive(false);

        Debug.Log("Черный экран убран");
    }

    Image CreateBlackScreen()
    {
        // Создаем новый Canvas для черного экрана поверх всего
        GameObject blackScreenCanvasObj = new GameObject("BlackScreenCanvas");
        Canvas blackScreenCanvas = blackScreenCanvasObj.AddComponent<Canvas>();
        blackScreenCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        blackScreenCanvas.sortingOrder = 9999; // Поверх всего

        // Создаем Image для черного экрана
        GameObject blackScreenObj = new GameObject("BlackScreen");
        blackScreenObj.transform.SetParent(blackScreenCanvasObj.transform);

        Image screenImage = blackScreenObj.AddComponent<Image>();
        screenImage.color = Color.black;

        // Растягиваем на весь экран
        RectTransform rect = blackScreenObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return screenImage;
    }
}