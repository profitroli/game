using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParliamentConstructor : MonoBehaviour
{
    [System.Serializable]
    public class ConstructionSlot
    {
        public TextMeshProUGUI slotText; // Ссылка на текст в белом квадрате
        public string[] options;         // Список вариантов (например: "110 депутатов", "64 члена")
        public int correctAnswerIndex;   // Номер правильного ответа в списке
        [HideInInspector] public int currentIndex = -1; // Что выбрал игрок
    }

    public ConstructionSlot[] slots;
    public TextMeshProUGUI statusText;

    // Метод для кнопок-квадратов
    public void ClickSlot(int slotIndex)
    {
        ConstructionSlot slot = slots[slotIndex];
        slot.currentIndex = (slot.currentIndex + 1) % slot.options.Length;
        slot.slotText.text = slot.options[slot.currentIndex];
        slot.slotText.color = Color.black; // Сбрасываем цвет при смене
    }

    public void CheckResults()
    {// 1. Сначала проверяем, все ли слоты заполнены
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentIndex == -1)
            {
                statusText.text = "<color=black>Сначала выберите значения во всех пустых квадратах!</color>";
                return; // Прерываем выполнение метода, проверка не начнется
            }
        }
        int correctCount = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentIndex == slots[i].correctAnswerIndex)
            {
                correctCount++;
                slots[i].slotText.color = Color.green; // Подсвечиваем верный
            }
            else
            {
                slots[i].slotText.color = Color.red;   // Подсвечиваем ошибку
            }
        }

        if (correctCount == slots.Length)
        {
            statusText.text = "Правильно! Парламент сформирован верно.";
            statusText.color = Color.forestGreen;
        }
        else
        {
            statusText.text = $"Ошибки в схеме! Верно: {correctCount} из {slots.Length}";
            statusText.color = Color.red;
        }
    }

    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}