using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NewspaperEditor : MonoBehaviour
{
    [System.Serializable]
    public class NewspaperSlot
    {
        public string correctContent;
        public TextMeshProUGUI slotText;
        public Button slotButton;
        [HideInInspector] public string currentContent = "";
        [HideInInspector] public string initialText; // Сюда сохраним "Название", "Город" и т.д.
    }

    public List<NewspaperSlot> slots;
    public Button publishButton;
    public TextMeshProUGUI statusText;

    private string selectedMaterial = "";

    void Start()
    {
        if (publishButton != null) publishButton.interactable = false;

        // ЗАПОМИНАЕМ ИЗНАЧАЛЬНЫЙ ТЕКСТ
        foreach (var s in slots)
        {
            if (s.slotText != null)
                s.initialText = s.slotText.text;
        }
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

        CheckIfAllFilled();
    }

    void CheckIfAllFilled()
    {
        foreach (var s in slots) if (string.IsNullOrEmpty(s.currentContent)) return;
        if (publishButton != null) publishButton.interactable = true;
    }

    public void FinalCheck()
    {
        foreach (var s in slots)
        {
            ColorBlock cb = s.slotButton.colors;
            if (s.currentContent == s.correctContent)
                cb.normalColor = Color.green;
            else
                cb.normalColor = Color.red;

            s.slotButton.colors = cb;
        }
    }

    // ТЕПЕРЬ СБРОС ВОЗВРАЩАЕТ ТЕКСТ К ИСХОДНОМУ
    public void ResetEditor()
    {
        selectedMaterial = "";
        foreach (var s in slots)
        {
            s.currentContent = "";
            s.slotText.text = s.initialText; // ВОЗВРАЩАЕМ ТО, ЧТО БЫЛО (например, "Название")

            ColorBlock cb = s.slotButton.colors;
            cb.normalColor = Color.white;
            s.slotButton.colors = cb;
        }
        if (publishButton != null) publishButton.interactable = false;
        statusText.text = "Макет сброшен.";
    }

public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}