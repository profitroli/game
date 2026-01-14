using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DropZone[] zones;
    public TextMeshProUGUI statusText;
    public int totalCountries; // Укажи здесь общее количество стран в игре
    public string originalStatusText;

    void Start()
    {
        // Запоминаем текст, который ты написала в Unity перед запуском
        if (statusText != null)
        {
            originalStatusText = statusText.text;
        }
    }
    public void CheckResult()
    {
        int placedItemsCount = 0;
        bool allGood = true;

        // 1. Считаем, сколько всего карточек положили в зоны
        foreach (var zone in zones)
        {
            DragItem[] itemsInZone = zone.GetComponentsInChildren<DragItem>();
            placedItemsCount += itemsInZone.Length;

            // Проверяем правильность
            foreach (var item in itemsInZone)
            {
                if (item.type != zone.zoneType) allGood = false;
            }
        }

        // 2. Проверяем условия
        if (placedItemsCount == 0)
        {
            statusText.text = "Сначала распредели!";
            statusText.color = Color.purple;
        }
        else if (placedItemsCount < totalCountries)
        {
            statusText.text = "Размести все карточки!";
            statusText.color = Color.purple;
        }
        else
        {
            // Если всё заполнено, показываем результат
            statusText.text = allGood ? "Молодец!!" : "Не правильно, подумай ещё";
            statusText.color = allGood ? Color.green : Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Функция для кнопки "Сброс"
    public void ResetGame()
    {
        // Находим вообще все карточки в сцене и вызываем их метод сброса
        DragItem[] allCards = FindObjectsOfType<DragItem>();
        foreach (var card in allCards)
        {
            card.ResetCard();
        }
        statusText.text = originalStatusText;
        statusText.color = Color.black;// Очищаем текст
    }
}