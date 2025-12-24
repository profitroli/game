using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestionToggleController : MonoBehaviour
{
    [Header("Список вопросов")]
    public Toggle[] questionToggles; // Используем массив вместо списка для простоты настройки в инспекторе

    [Header("Настройки сохранения")]
    public string saveKeyPrefix = "Question_";
    public bool autoFindToggles = false; // Опция для автоматического поиска

    void Start()
    {
        InitializeToggles();
        LoadToggleStates();
        SetupToggleListeners();

        Debug.Log($"QuestionToggleController инициализирован. Найдено тогглов: {questionToggles.Length}");
    }

    void InitializeToggles()
    {
        if (autoFindToggles)
        {
            // Автоматически находим все тогглы в сцене
            Toggle[] allToggles = FindObjectsOfType<Toggle>(true); // true - ищем неактивные тоже
            List<Toggle> questionToggleList = new List<Toggle>();

            foreach (Toggle toggle in allToggles)
            {
                // Ищем только тогглы, которые не системные
                if (toggle.name.Contains("Question") ||
                    toggle.name.Contains("Вопрос") ||
                    toggle.gameObject.transform.parent != null &&
                    (toggle.gameObject.transform.parent.name.Contains("Question") ||
                     toggle.gameObject.transform.parent.name.Contains("Вопрос")))
                {
                    questionToggleList.Add(toggle);
                    Debug.Log($"Найден тоггл вопроса: {toggle.name}");
                }
            }

            questionToggles = questionToggleList.ToArray();
        }

        // Если все равно пусто, выводим предупреждение
        if (questionToggles == null || questionToggles.Length == 0)
        {
            Debug.LogWarning("Не найдены тогглы вопросов! Проверьте настройки.");
            questionToggles = new Toggle[0];
        }
    }

    void LoadToggleStates()
    {
        for (int i = 0; i < questionToggles.Length; i++)
        {
            if (questionToggles[i] != null)
            {
                string key = saveKeyPrefix + i.ToString();
                int savedValue = PlayerPrefs.GetInt(key, 0);
                bool isOn = savedValue == 1;

                // Важно: отключаем временно слушатель, чтобы не вызывать сохранение при загрузке
                questionToggles[i].SetIsOnWithoutNotify(isOn);

                // Обновляем визуал
                UpdateToggleAppearance(questionToggles[i], isOn);

                Debug.Log($"Загружен тоггл {i}: {isOn} (ключ: {key})");
            }
        }
    }

    void SetupToggleListeners()
    {
        for (int i = 0; i < questionToggles.Length; i++)
        {
            if (questionToggles[i] != null)
            {
                int index = i; // Локальная копия для замыкания

                // Удаляем старые слушатели
                questionToggles[i].onValueChanged.RemoveAllListeners();

                // Добавляем новый слушатель
                questionToggles[i].onValueChanged.AddListener((value) =>
                {
                    OnToggleValueChanged(index, value);
                });
            }
        }
    }

    void OnToggleValueChanged(int index, bool value)
    {
        Debug.Log($"Тоггл {index} изменен на: {value}");

        if (index >= 0 && index < questionToggles.Length && questionToggles[index] != null)
        {
            // Сохраняем
            string key = saveKeyPrefix + index.ToString();
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();

            // Обновляем визуал
            UpdateToggleAppearance(questionToggles[index], value);

            Debug.Log($"Сохранено: {key} = {value}");
        }
    }

    void UpdateToggleAppearance(Toggle toggle, bool isCompleted)
    {
        if (toggle == null) return;

        // Меняем цвет текста, если есть Text компонент
        Text label = toggle.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.color = isCompleted ? new Color(0.2f, 0.8f, 0.2f) : Color.white;
        }

        // Меняем цвет галочки
        Image checkmark = toggle.graphic as Image;
        if (checkmark != null)
        {
            checkmark.color = isCompleted ? Color.black : Color.white;
        }
    }

    // Метод для сброса прогресса (можно вызвать из кнопки UI)
    public void ResetProgress()
    {
        for (int i = 0; i < questionToggles.Length; i++)
        {
            string key = saveKeyPrefix + i.ToString();
            PlayerPrefs.DeleteKey(key);

            if (questionToggles[i] != null)
            {
                questionToggles[i].SetIsOnWithoutNotify(false);
                UpdateToggleAppearance(questionToggles[i], false);
            }
        }
        PlayerPrefs.Save();
        Debug.Log("Прогресс сброшен");
    }

    // Метод для принудительного сохранения всех состояний
    public void SaveAllStates()
    {
        for (int i = 0; i < questionToggles.Length; i++)
        {
            if (questionToggles[i] != null)
            {
                string key = saveKeyPrefix + i.ToString();
                PlayerPrefs.SetInt(key, questionToggles[i].isOn ? 1 : 0);
            }
        }
        PlayerPrefs.Save();
        Debug.Log("Все состояния сохранены");
    }

    void OnApplicationQuit()
    {
        SaveAllStates();
    }

    void OnDestroy()
    {
        SaveAllStates();
    }
}