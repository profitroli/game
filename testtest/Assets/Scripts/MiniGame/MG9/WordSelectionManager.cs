using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WordSelectionManager : MonoBehaviour
{
    [System.Serializable]
    public class WordElement
    {
        public string itemName;      // Название (например, "Войско")
        public Button wordButton;    // Кнопка в UI
        public bool isCorrectTarget; // Должно ли это слово быть выбрано?
        [HideInInspector] public bool isSelected = false;
    }

    public List<WordElement> words;
    public TextMeshProUGUI statusText;

    void Start()
    {
        foreach (var w in words)
        {
            WordElement current = w;
            current.wordButton.onClick.AddListener(() => OnWordClicked(current));
            current.wordButton.image.color = Color.white;
        }
        statusText.text = "Выберите то, что стало ОБЩИМ для ВКЛ и Польши:";
    }

    void OnWordClicked(WordElement w)
    {
        w.isSelected = !w.isSelected;
        // Подсветка при выборе (голубой цвет)
        w.wordButton.image.color = w.isSelected ? new Color(0.6f, 0.8f, 1f) : Color.white;
    }

    public void CheckResults()
    {
        int correctSelected = 0;
        int totalRequired = 0;
        bool hasErrors = false;

        foreach (var w in words)
        {
            if (w.isCorrectTarget) totalRequired++;

            if (w.isSelected)
            {
                if (w.isCorrectTarget)
                {
                    // Правильно выбранный элемент
                    w.wordButton.image.color = Color.green;
                    correctSelected++;
                }
                else
                {
                    // Ошибочно выбранный элемент
                    w.wordButton.image.color = Color.red;
                    hasErrors = true;
                }
            }
            else
            {
                // Если кнопка НЕ выбрана, возвращаем ей белый цвет (или серый)
                w.wordButton.image.color = Color.white;

                // Если игрок ЗАБЫЛ выбрать нужную кнопку
                if (w.isCorrectTarget) hasErrors = true;
            }
        }

        if (!hasErrors && correctSelected == totalRequired)
        {
            statusText.text = "Верно! ВКЛ сохранило автономию, но монарх и сейм стали общими.";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "Есть ошибки или пропущенные элементы!";
            statusText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        foreach (var w in words)
        {
            w.isSelected = false;
            w.wordButton.image.color = Color.white;
        }
        statusText.text = "Выберите то, что стало ОБЩИМ для ВКЛ и Польши";
        statusText.color = Color.black;
    }
}