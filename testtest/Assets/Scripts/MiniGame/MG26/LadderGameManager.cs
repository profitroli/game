using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LadderGameManager : MonoBehaviour
{
    public StepSlot[] slots;            // Все слоты на лестнице
    public TextMeshProUGUI statusText;  // Текст-помощник
    public GameObject finishPanel;      // Панель финала (если она у тебя есть)

    void Start()
    {
        // Начальный текст по материалам лекции
        statusText.text = "Расставь титулы от древности (внизу) к современности (вверху)";
    }

    public void CheckLadder()
    {
        int correctCount = 0;
        int filledSlots = 0;

        foreach (var slot in slots)
        {
            // Проверяем, есть ли что-то в слоте
            if (slot != null && slot.transform.childCount > 0)
            {
                filledSlots++;

                // Ищем скрипт TitleStep на плашке внутри слота
                TitleStep item = slot.GetComponentInChildren<TitleStep>();

                if (item != null)
                {
                    // ВНИМАНИЕ: Вот исправленная часть для цвета!
                    // Ищем картинку на самом объекте ИЛИ на его родителе
                    Image itemImage = item.GetComponent<Image>();
                    if (itemImage == null) itemImage = item.GetComponentInParent<Image>();

                    // Сверяем ID плашки и ID слота
                    if (item.titleID == slot.slotID)
                    {
                        correctCount++;
                        if (itemImage != null) itemImage.color = Color.green; // Верно — зеленый
                    }
                    else
                    {
                        if (itemImage != null) itemImage.color = Color.red;   // Ошибка — красный
                    }
                }
            }
        }

        // Логика вывода текста
        if (filledSlots == 0)
        {
            statusText.text = "Поставь хотя бы одну плашку на ступеньку!";
        }
        else if (filledSlots < slots.Length)
        {
            statusText.text = $"Ты расставил не всё! Правильно: {correctCount} из {filledSlots}.";
        }
        else
        {
            if (correctCount == slots.Length)
            {
                // Поздравление по итогам лекции
                statusText.text = "Молодец!Вы идеально изучили эволюцию власти в Беларуси!";
                if (finishPanel != null) finishPanel.SetActive(true);
            }
            else
            {
                statusText.text = $"Почти! Правильно: {correctCount} из {slots.Length}. Попробуй поменять красные.";
            }
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}