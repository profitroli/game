using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OddOneOutManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizGroup
    {
        public string groupName;       // Название группы (для инспектора)
        public Button[] buttons;       // 4 кнопки этой группы
        public int correctOddIndex;    // Индекс лишнего слова (0, 1, 2 или 3)
        [HideInInspector] public int selectedIndex = -1; // Что выбрал игрок
    }

    public List<QuizGroup> quizGroups; // Список всех групп вопросов
    public TextMeshProUGUI statusText;

    void Start()
    {
        // Настраиваем кнопки при старте
        for (int g = 0; g < quizGroups.Count; g++)
        {
            int groupIdx = g;
            for (int b = 0; b < quizGroups[g].buttons.Length; b++)
            {
                int btnIdx = b;
                quizGroups[g].buttons[b].onClick.AddListener(() => SelectButton(groupIdx, btnIdx));
            }
        }
    }

    // Логика выбора кнопки
    public void SelectButton(int groupIdx, int btnIdx)
    {
        // Сбрасываем цвет всех кнопок в группе
        foreach (Button btn in quizGroups[groupIdx].buttons)
        {
            btn.image.color = Color.white;
        }

        // Подсвечиваем выбранную кнопку синим (или любым другим цветом выбора)
        quizGroups[groupIdx].buttons[btnIdx].image.color = new Color(0.7f, 0.8f, 1f);
        quizGroups[groupIdx].selectedIndex = btnIdx;
    }

    public void CheckResults()
    {
        int correctCount = 0;
        bool allSelected = true;

        // Проверяем, во всех ли группах сделан выбор
        foreach (var group in quizGroups)
        {
            if (group.selectedIndex == -1) allSelected = false;
        }

        if (!allSelected)
        {
            statusText.text = "Выберите лишнее слово в каждой группе!";
            statusText.color = Color.purple;
            return;
        }

        // Сверяем результаты
        foreach (var group in quizGroups)
        {
            if (group.selectedIndex == group.correctOddIndex)
            {
                correctCount++;
                group.buttons[group.selectedIndex].image.color = Color.green;
            }
            else
            {
                group.buttons[group.selectedIndex].image.color = Color.red;
            }
        }

        if (correctCount == quizGroups.Count)
        {
            statusText.text = "Верно! Вы нашли всех лишних!";
            statusText.color = Color.forestGreen;
        }
        else
        {
            statusText.text = "Есть ошибки, попробуйте еще раз";
            statusText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        foreach (var group in quizGroups)
        {
            group.selectedIndex = -1;
            foreach (Button btn in group.buttons)
            {
                btn.image.color = Color.white;
            }
        }
        statusText.text = "Выберите не подходящее в каждой грппе";
        statusText.color = Color.black;
    }
}