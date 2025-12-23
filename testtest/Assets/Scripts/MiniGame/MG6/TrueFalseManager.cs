using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrueFalseManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public ToggleAnswer toggleBtn; // Ссылка на кнопку-переключатель
        public bool isActuallyTrue;    // Правильный ответ из истории
    }

    public List<QuestionData> questions;
    public TextMeshProUGUI statusText;

    public void CheckAllAnswers()
    {
        int correctCount = 0;
        bool allAnswered = true;

        foreach (var q in questions)
        {
            if (q.toggleBtn.currentState == ToggleAnswer.State.None) allAnswered = false;
        }

        if (!allAnswered)
        {
            statusText.text = "Ответьте на все вопросы!";
            return;
        }

        foreach (var q in questions)
        {
            bool userSaidTrue = (q.toggleBtn.currentState == ToggleAnswer.State.True);

            if (userSaidTrue == q.isActuallyTrue)
            {
                correctCount++;
                q.toggleBtn.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                q.toggleBtn.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
        }

        statusText.text = (correctCount == questions.Count) ? "Великолепно!" : $"Верно: {correctCount} из {questions.Count}";
        statusText.color = Color.green;
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        foreach (var q in questions) q.toggleBtn.ResetButton();
        statusText.text = "Крещение Руси: верны ли утверждения?";
        statusText.color = Color.black;
    }
}