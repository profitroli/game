using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasyPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleSlot
    {
        public Image slotImage;           // Объект картинки в сетке
        public List<Sprite> allVariants;  // Список всех 4-х кусочков
        public int correctIndex;          // Номер правильного кусочка (0, 1, 2 или 3)
        [HideInInspector] public int currentIndex = 0;
    }

    public List<PuzzleSlot> slots;
    public TMP_InputField finalWordInput;
    public TextMeshProUGUI resultText;
    public string correctWord ;

    // Вызывается при клике на саму картинку в сетке
    public void ChangeImage(int slotIndex)
    {
        PuzzleSlot s = slots[slotIndex];
        s.currentIndex = (s.currentIndex + 1) % s.allVariants.Count; // Листаем по кругу
        s.slotImage.sprite = s.allVariants[s.currentIndex];
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void CheckResult()
    {
        int correctCount = 0;
        foreach (var s in slots)
        {
            if (s.currentIndex == s.correctIndex) correctCount++;
        }

        // 1. Очистка текста от невидимых символов TMP (заменяем \u200B на пустоту)
        string cleanUserText = finalWordInput.text.Replace("\u200B", "").Trim().ToLower();
        string cleanCorrectWord = correctWord.Trim().ToLower();

        bool wordOk = cleanUserText == cleanCorrectWord;

        // 2. Лог в консоль (поможет понять, в чем ошибка при тесте)
        Debug.Log($"Собрано кусков: {correctCount}/{slots.Count}. Слово верно: {wordOk}");

        if (correctCount == slots.Count && wordOk)
        {
            resultText.text = "Молодец! Всё правильно";
            resultText.color = Color.green;
        }
        else
        {
            // Уточняем для себя, где ошибка (можно убрать потом)
            if (correctCount != slots.Count && !wordOk)
                resultText.text = "Ошибка в пазле и в слове";
            else if (correctCount != slots.Count)
                resultText.text = "Картинка собрана неверно";
            else
                resultText.text = "Слово введено неверно";

            resultText.color = Color.red;
        }
    }

    public void ResetPuzzle()
    {
        foreach (var s in slots)
        {
            s.currentIndex = Random.Range(0, s.allVariants.Count); // Перемешиваем
            s.slotImage.sprite = s.allVariants[s.currentIndex];
        }
        finalWordInput.text = "";
        resultText.text = "Соберите пазл и разгадайте его";
        resultText.color = Color.black;
    }
}