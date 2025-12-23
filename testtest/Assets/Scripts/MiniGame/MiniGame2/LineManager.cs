using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LineManager : MonoBehaviour
{
    [Header("Настройки UI")]
    public GameObject linePrefab;      // Префаб Image (Pivot 0, 0.5)
    public RectTransform canvasRect;  // Ссылка на сам Canvas
    public TextMeshProUGUI statusText; // Текст для вывода сообщений

    [Header("Настройки Игры")]
    public int totalConnectionsNeeded = 7; // Сколько пар нужно соединить (у нас 7)
    public string mainMenuSceneName = "MainMenu"; // Название главной сцены

    private GameObject currentLine;
    private ConnectionDot startDot;

    // Списки для хранения данных о созданных соединениях
    private List<GameObject> activeLines = new List<GameObject>();
    private List<KeyValuePair<ConnectionDot, ConnectionDot>> connections = new List<KeyValuePair<ConnectionDot, ConnectionDot>>();

    /// <summary>
    /// Начинает рисование линии от точки
    /// </summary>
    public void StartDrawing(ConnectionDot dot)
    {
        startDot = dot;

        // Создаем линию как ребенка Canvas
        currentLine = Instantiate(linePrefab, canvasRect);

        // Перемещаем линию в самый верх иерархии внутри Canvas, 
        // чтобы она была визуально ПОД точками
        currentLine.transform.SetAsLastSibling();

        // ВАЖНО: Отключаем Raycast у линии, чтобы она не перекрывала вторую точку
        if (currentLine.TryGetComponent<Image>(out Image img))
        {
            img.raycastTarget = false;
        }
    }

    void Update()
    {
        // Если мы сейчас тянем линию, обновляем её положение за мышкой
        if (currentLine != null)
        {
            UpdateLine(Input.mousePosition);
        }
    }

    /// <summary>
    /// Завершает рисование линии
    /// </summary>
    public void EndDrawing(ConnectionDot endDot)
    {
        // Проверяем: отпустили ли над точкой, не та же ли это точка и с другой ли она стороны
        if (endDot != null && endDot != startDot && endDot.isLeft != startDot.isLeft)
        {
            // Фиксируем линию в конечной точке
            UpdateLine(endDot.transform.position);

            activeLines.Add(currentLine);
            connections.Add(new KeyValuePair<ConnectionDot, ConnectionDot>(startDot, endDot));

            currentLine = null;
        }
        else
        {
            // Если условия не соблюдены (отпустили в пустоту) — удаляем линию
            if (currentLine != null) Destroy(currentLine);
            currentLine = null;
        }
    }

    /// <summary>
    /// Математический расчет положения, длины и поворота линии
    /// </summary>
    void UpdateLine(Vector3 targetWorldPos)
    {
        if (currentLine == null) return;

        RectTransform rt = currentLine.GetComponent<RectTransform>();

        // Конвертируем мировые координаты в локальные координаты Canvas
        Vector2 localStartPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, startDot.transform.position, null, out localStartPos);

        Vector2 localTargetPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, targetWorldPos, null, out localTargetPos);

        // Ставим начало линии в стартовую точку
        rt.anchoredPosition = localStartPos;

        // Вычисляем вектор направления
        Vector2 direction = localTargetPos - localStartPos;

        // Устанавливаем длину (ширину) линии
        rt.sizeDelta = new Vector2(direction.magnitude, 5f); // 5f — толщина линии

        // Устанавливаем угол поворота
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Проверка результатов (для кнопки "Проверить")
    /// </summary>
    public void CheckResults()
    {
        if (statusText == null)
            statusText.text = "Соотнесите периоды и их символы" ;

        if (connections.Count == 0)
        {
            statusText.text = "Сначала нарисуйте линии!";
            statusText.color = Color.black;
            return;
        }

        if (connections.Count < totalConnectionsNeeded)
        {
            statusText.text = "Соедините все этапы!";
            statusText.color = Color.white;
            return;
        }

        int correctCount = 0;
        foreach (var pair in connections)
        {
            // Сравниваем ID левой и правой точки
            if (pair.Key.id == pair.Value.id)
            {
                correctCount++;
            }
        }

        if (correctCount == totalConnectionsNeeded)
        {
            statusText.text = "Молодец!!";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "Неправильно, попробуй ещё раз";
            statusText.color = Color.red;
        }
    }

    /// <summary>
    /// Полный сброс игры (для кнопки "Сброс")
    /// </summary>
    public void ResetGame()
    {
        // Удаляем все объекты линий
        foreach (GameObject line in activeLines)
        {
            if (line != null) Destroy(line);
        }

        activeLines.Clear();
        connections.Clear();

        if (statusText != null) { statusText.text = "Соотнесите периоды и их символы"; statusText.color = Color.black; }
    }

    /// <summary>
    /// Переход в меню (для кнопки "Назад")
    /// </summary>
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}