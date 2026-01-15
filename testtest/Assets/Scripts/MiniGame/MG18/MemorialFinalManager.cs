using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemorialFinalManager : MonoBehaviour
{
    [System.Serializable]
    public class MemorialBlock
    {
        public Image iconImage;           // Картинка на экране
        public List<Sprite> allSprites;   // Список всех мемориалов (Брест, Хатынь и т.д.)
        public int correctSpriteIndex;    // Индекс правильного фото в списке
        public TMP_InputField nameInput;  // Поле ввода
        public string correctName;        // Верный текст (например, "Хатынь")

        [HideInInspector] public int currentIndex = 0;
    }

    public List<MemorialBlock> blocks;
    public TextMeshProUGUI statusText;

    private string originalMessage;

    void Start()
    {
        // Запоминаем исходный текст задания, как ты просила
        if (statusText != null) originalMessage = statusText.text;
        ResetGame();
    }

    // Метод для кнопок-картинок (меняет визуал, но не проверяет)
    public void ChangeMemorial(int blockIndex)
    {
        MemorialBlock b = blocks[blockIndex];
        b.currentIndex = (b.currentIndex + 1) % b.allSprites.Count;
        b.iconImage.sprite = b.allSprites[b.currentIndex];
    }

    // ГЛАВНАЯ ПРОВЕРКА (на кнопке "Проверить")
    public void CheckAll()
    {
        int score = 0;

        foreach (var b in blocks)
        {
            // Чистим текст от невидимых символов TMP
            string userText = b.nameInput.text.Replace("\u200B", "").Trim().ToLower();
            string targetText = b.correctName.Trim().ToLower();

            bool isImageRight = (b.currentIndex == b.correctSpriteIndex);
            bool isWordRight = (userText == targetText);

            if (isImageRight && isWordRight)
            {
                score++;
                b.nameInput.image.color = Color.green;
            }
            else
            {
                b.nameInput.image.color = Color.red;
            }
        }

        if (score == blocks.Count)
        {
            statusText.text = "Молодец! Всё правильно";
            statusText.color = Color.forestGreen;
        }
        else
        {
            statusText.text = "ПЕРЕДЕЛАЙТЕ! Есть ошибки";
            statusText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        foreach (var b in blocks)
        {
            b.currentIndex = Random.Range(0, b.allSprites.Count);
            b.iconImage.sprite = b.allSprites[b.currentIndex];
            b.nameInput.text = "";
            b.nameInput.image.color = Color.white;
        }

        if (statusText != null)
        {
            statusText.text = originalMessage;
            statusText.color = Color.black;
        }
    }
}