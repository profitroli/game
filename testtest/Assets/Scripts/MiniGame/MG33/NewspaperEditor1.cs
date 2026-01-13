using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewspaperEditor1 : MonoBehaviour
{
    [System.Serializable]
    public class NewspaperSlot
    {
        public string correctContent;
        public TextMeshProUGUI slotText;
        public Button slotButton;
        [HideInInspector] public string currentContent = "";
        [HideInInspector] public string initialText;
    }

    public List<NewspaperSlot> slots;
    public Button publishButton; // Твоя кнопка "В печать"
    public TextMeshProUGUI statusText; // Текст состояния (сверху)
    public TextMeshProUGUI resultVerdictText; // Новое поле для "Одобрено/Переделайте"

    private string selectedMaterial = "";

    void Start()
    {
        if (publishButton != null) publishButton.interactable = false;
        if (resultVerdictText != null) resultVerdictText.text = "";

        foreach (var s in slots)
        {
            if (s.slotText != null)
                s.initialText = s.slotText.text;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SelectMaterial(string content)
    {
        selectedMaterial = content;
        statusText.text = "Выбрано: " + content;
    }

    public void PlaceInSlot(int index)
    {
        if (string.IsNullOrEmpty(selectedMaterial)) return;

        slots[index].currentContent = selectedMaterial;
        slots[index].slotText.text = selectedMaterial;
        selectedMaterial = "";

        // Сбрасываем цвета кнопок при редактировании
        ColorBlock cb = slots[index].slotButton.colors;
        cb.normalColor = Color.white;
        cb.selectedColor = Color.white;
        slots[index].slotButton.colors = cb;

        CheckIfAllFilled();
    }

    void CheckIfAllFilled()
    {
        foreach (var s in slots) if (string.IsNullOrEmpty(s.currentContent)) return;
        if (publishButton != null) publishButton.interactable = true;
    }

    // МЕТОД ДЛЯ КНОПКИ "В ПЕЧАТЬ"
    public void FinalCheck()
    {
        int correctCount = 0;

        foreach (var s in slots)
        {
            ColorBlock cb = s.slotButton.colors;
            if (s.currentContent.Trim() == s.correctContent.Trim())
            {
                cb.normalColor = Color.green;
                cb.selectedColor = Color.green;
                correctCount++;
            }
            else
            {
                cb.normalColor = Color.red;
                cb.selectedColor = Color.red;
            }
            s.slotButton.colors = cb;
        }

        if (correctCount == slots.Count)
        {
            resultVerdictText.text = "ВИЗИТКА ОДОБРЕНА!";
            resultVerdictText.color = Color.green;
        }
        else
        {
            resultVerdictText.text = "ПЕРЕДЕЛАЙТЕ! ЕСТЬ ОШИБКИ";
            resultVerdictText.color = Color.red;
        }
    }

    public void ResetEditor()
    {
        selectedMaterial = "";
        if (resultVerdictText != null) resultVerdictText.text = "";

        foreach (var s in slots)
        {
            s.currentContent = "";
            s.slotText.text = s.initialText;

            ColorBlock cb = s.slotButton.colors;
            cb.normalColor = Color.white;
            cb.selectedColor = Color.white;
            s.slotButton.colors = cb;
        }
        if (publishButton != null) publishButton.interactable = false;
        statusText.text = "Выберите материал и кликните по зоне визитки";
        statusText.color = Color.black;
    }
}