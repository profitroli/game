using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotoManager : MonoBehaviour
{
    [Header("Настройки объектов")]
    public GameObject photoObject; // Фотка, которая должна появиться
    public float autoHideTime = 10f; // Время до исчезновения

    private Coroutine timerCoroutine;

    void Start()
    {
        // Убедимся, что при старте фото скрыто
        if (photoObject != null)
            photoObject.SetActive(false);
    }

    // Этот метод мы привяжем к кнопке
    public void OnButtonClick()
    {
        if (photoObject == null) return;

        // Если фото уже активно — скрываем вручную
        if (photoObject.activeSelf)
        {
            Hide();
        }
        // Если скрыто — показываем и запускаем таймер
        else
        {
            Show();
        }
    }

    private void Show()
    {
        photoObject.SetActive(true);

        // Сбрасываем старый таймер, если он шел
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        // Запускаем новый отсчет на 10 секунд
        timerCoroutine = StartCoroutine(Countdown());
    }

    private void Hide()
    {
        photoObject.SetActive(false);
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(autoHideTime);
        photoObject.SetActive(false);
    }
}