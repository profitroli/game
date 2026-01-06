using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecisionGame : MonoBehaviour
{
    [System.Serializable]
    public class DecisionNode
    {
        public string situation;   // Описание ситуации
        public string optionA;     // Текст на кнопке А
        public string optionB;     // Текст на кнопке Б
        public bool isCorrectA;    // Какой вариант исторически верный
        public string resultInfo;  // Историческая справка после выбора
    }

    public List<DecisionNode> nodes;
    public TextMeshProUGUI situationText;
    public TextMeshProUGUI btnAText;
    public TextMeshProUGUI btnBText;
    public TextMeshProUGUI infoText;

    private int currentStep = 0;

    void Start() => ShowNode();

    public void MakeDecision(bool choiceA)
    {
        if (currentStep >= nodes.Count) return;

        bool wasCorrect = (choiceA == nodes[currentStep].isCorrectA);

        if (wasCorrect)
        {
            infoText.text = "ПРАВИЛЬНО: " + nodes[currentStep].resultInfo;
            infoText.color = Color.green;
            currentStep++;
            Invoke("ShowNode", 9f); // Даем время прочитать справку
        }
        else
        {
            infoText.text = "ОШИБКА: Этот путь не соответствует истории РБ.";
            infoText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void ShowNode()
    {
        if (currentStep < nodes.Count)
        {
            situationText.text = nodes[currentStep].situation;
            btnAText.text = nodes[currentStep].optionA;
            btnBText.text = nodes[currentStep].optionB;
            infoText.text = "Сделайте выбор...";
            infoText.color = Color.black;
        }
        else
        {
            infoText.text = "ПОБЕДА! Вы прошли путь развития независимой Беларуси.";
            infoText.color = Color.green;
            situationText.text = "";
            btnAText.text = "";
            btnBText.text = "";

        }
    }
}