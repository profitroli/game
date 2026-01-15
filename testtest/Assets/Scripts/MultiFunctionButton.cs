using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DoubleClickNavigation : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Названия сцен")]
    public string singleClickScene; // Один клик (Меню/Лекция)
    public string doubleClickScene; // Двойной клик (След./Пред. игра)

    [Header("Настройки")]
    public GameObject tooltipText;
    private float delay = 0.3f; // Окно ожидания второго клика
    private int tapCount = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        tapCount++;

        if (tapCount == 1)
        {
            // Ждем немного, не нажмет ли пользователь второй раз
            Invoke("SingleClickAction", delay);
        }
        else if (tapCount == 2)
        {
            // Если нажали второй раз — отменяем одиночный клик и делаем двойной
            CancelInvoke("SingleClickAction");
            ExecuteDoubleClick();
        }
    }

    private void SingleClickAction()
    {
        tapCount = 0;
        if (!string.IsNullOrEmpty(singleClickScene))
        {
            SceneManager.LoadScene(singleClickScene);
        }
    }

    private void ExecuteDoubleClick()
    {
        tapCount = 0;
        if (!string.IsNullOrEmpty(doubleClickScene))
        {
            SceneManager.LoadScene(doubleClickScene);
        }
    }

    // Управление подсказкой при наведении
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipText != null) tooltipText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipText != null) tooltipText.SetActive(false);
    }
}