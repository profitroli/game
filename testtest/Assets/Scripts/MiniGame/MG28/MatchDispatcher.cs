using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MatchDispatcherPro : MonoBehaviour
{
    [System.Serializable]
    public class MatchPair
    {
        public Button problemButton;
        public Button departmentButton;
        [HideInInspector] public Button currentAssignedDepartment; // Текущий выбор игрока
    }

    public List<MatchPair> pairs;
    public TextMeshProUGUI statusText;

    private Button selectedProblem;
    private Color defaultColor = Color.white;
    private Color selectedColor = Color.yellow;

    void Start()
    {
        statusText.text = "Соедините проблемы с ведомствами, затем нажмите 'Проверить'";

        // Настраиваем кнопки проблем
        foreach (var pair in pairs)
        {
            Button pBtn = pair.problemButton;
            pBtn.onClick.AddListener(() => SelectProblem(pBtn));
        }

        // Настраиваем кнопки ведомств
        // Для простоты: все кнопки ведомств должны иметь одинаковый метод
        // В инспекторе нужно будет убедиться, что все кнопки из списка departmentButton 
        // вызывают один и тот же метод через код ниже
        foreach (var pair in pairs)
        {
            Button dBtn = pair.departmentButton;
            dBtn.onClick.AddListener(() => SelectDepartment(dBtn));
        }
    }

    void SelectProblem(Button btn)
    {
        if (selectedProblem != null) selectedProblem.GetComponent<Image>().color = defaultColor;
        selectedProblem = btn;
        btn.GetComponent<Image>().color = selectedColor;
    }

    void SelectDepartment(Button btn)
    {
        if (selectedProblem == null)
        {
            statusText.text = "Сначала выберите проблему слева!";
            return;
        }

        // Запоминаем выбор для конкретной проблемы
        foreach (var pair in pairs)
        {
            if (pair.problemButton == selectedProblem)
            {
                // Если это ведомство уже было выбрано кем-то другим, очищаем у того
                foreach (var p in pairs) if (p.currentAssignedDepartment == btn) p.currentAssignedDepartment = null;

                pair.currentAssignedDepartment = btn;
                statusText.text = "Связь установлена. Ищите следующую пару.";
                break;
            }
        }

        selectedProblem.GetComponent<Image>().color = Color.cyan; // Помечаем, что пара выбрана
        selectedProblem = null;
    }

    public void CheckResults()
    {
        int correct = 0;
        int total = pairs.Count;

        foreach (var pair in pairs)
        {
            if (pair.currentAssignedDepartment == null) continue;

            if (pair.currentAssignedDepartment == pair.departmentButton)
            {
                correct++;
                pair.problemButton.GetComponent<Image>().color = Color.green;
                pair.currentAssignedDepartment.GetComponent<Image>().color = Color.green;
            }
            else
            {
                pair.problemButton.GetComponent<Image>().color = Color.red;
                pair.currentAssignedDepartment.GetComponent<Image>().color = Color.red;
            }
        }

        statusText.text = $"ИТОГ: Правильно {correct} из {total}. " + (correct == total ? "Вы отличный управленец!" : "Попробуйте еще раз.");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}