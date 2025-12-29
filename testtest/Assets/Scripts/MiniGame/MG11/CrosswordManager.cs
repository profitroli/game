using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrosswordManager : MonoBehaviour
{
    [System.Serializable]
    public class WordLine
    {
        public string correctWord;
        public List<TMP_InputField> cells;
    }

    public List<WordLine> words;
    public TextMeshProUGUI resultText;

    // Кнопка ПРОВЕРИТЬ
    public void CheckCrossword()
    {
        int correctWordsCount = 0;

        foreach (var line in words)
        {
            string playerWord = "";
            foreach (var cell in line.cells)
            {
                playerWord += cell.text.Trim().ToUpper();
            }

            if (playerWord == line.correctWord.ToUpper())
            {
                correctWordsCount++;
                SetLineColor(line, new Color(0.6f, 1f, 0.6f)); // Светло-зеленый
            }
            else
            {
                SetLineColor(line, new Color(1f, 0.6f, 0.6f)); // Светло-красный
            }
        }

        resultText.text = (correctWordsCount == words.Count)
            ? "Победа! Все слова верны."
            : $"Верно слов: {correctWordsCount} из {words.Count}";
    }

    
    public void ResetCrossword()
    {
        foreach (var line in words)
        {
            foreach (var cell in line.cells)
            {
                cell.text = "";
                cell.image.color = Color.white;
            }
        }
        resultText.text = "Разгадайте кроссворд, вопросы в указателях";
        resultText.color = Color.black;
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void SetLineColor(WordLine line, Color color)
    {
        foreach (var cell in line.cells)
        {
            cell.image.color = color;
        }
    }
}