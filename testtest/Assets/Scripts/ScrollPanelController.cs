using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ScrollPanelController : MonoBehaviour, IScrollHandler
{
    [Header("Настройки прокрутки")]
    [Tooltip("Скорость прокрутки. Чем больше значение, тем быстрее двигается панель")]
    [SerializeField] private float scrollSpeed = 20f;

    [Header("Ограничения движения")]
    [Tooltip("Минимальная позиция по оси Y")]
    [SerializeField] private float minYPosition = -300f;

    [Tooltip("Максимальная позиция по оси Y")]
    [SerializeField] private float maxYPosition = 300f;

    [Header("Дополнительные настройки")]
    [Tooltip("Инвертировать направление прокрутки?")]
    [SerializeField] private bool invertScrollDirection = false;

    [Tooltip("Показывать границы при движении?")]
    [SerializeField] private bool showDebugBounds = true;

    [Header("Сохранение позиции")]
    [Tooltip("Сохранять позицию при переключении сцен/страниц?")]
    [SerializeField] private bool savePosition = true;

    [Tooltip("Уникальный ключ для сохранения позиции. Оставьте пустым для автоматической генерации")]
    [SerializeField] private string saveKey = "";

    // Ссылка на RectTransform панели
    private RectTransform panelRectTransform;

    // Текущая позиция
    private Vector3 currentPosition;

    // Для визуализации границ (только в редакторе)
#if UNITY_EDITOR
    private Vector3[] worldCorners = new Vector3[4];
#endif

    void Start()
    {
        // Получаем компонент RectTransform
        panelRectTransform = GetComponent<RectTransform>();

        // Генерируем ключ сохранения если не задан
        if (string.IsNullOrEmpty(saveKey))
        {
            saveKey = GenerateSaveKey();
        }

        // Восстанавливаем сохраненную позицию или используем текущую
        LoadSavedPosition();

        Debug.Log("Scroll Panel Controller инициализирован");
        Debug.Log($"Границы движения: от {minYPosition} до {maxYPosition}");
        Debug.Log($"Ключ сохранения: {saveKey}");
    }

    void OnEnable()
    {
        // При повторной активации объекта (например, при возвращении на страницу)
        // можно загрузить позицию снова
        if (savePosition)
        {
            LoadSavedPosition();
        }
    }

    void OnDisable()
    {
        // При деактивации объекта сохраняем позицию
        if (savePosition)
        {
            SaveCurrentPosition();
        }
    }

    void OnDestroy()
    {
        // При уничтожении объекта тоже сохраняем позицию
        if (savePosition)
        {
            SaveCurrentPosition();
        }
    }

    // Генерация уникального ключа для сохранения
    private string GenerateSaveKey()
    {
        // Используем путь в иерархии для уникальности
        string path = transform.GetPath();
        return $"ScrollPanelPos_{path.GetHashCode()}";
    }

    // Сохранение текущей позиции
    private void SaveCurrentPosition()
    {
        if (!savePosition) return;

        PlayerPrefs.SetFloat(saveKey, currentPosition.y);
        PlayerPrefs.Save();

        Debug.Log($"Позиция сохранена: {currentPosition.y}");
    }

    // Загрузка сохраненной позиции
    private void LoadSavedPosition()
    {
        if (!savePosition) return;

        if (PlayerPrefs.HasKey(saveKey))
        {
            float savedY = PlayerPrefs.GetFloat(saveKey, 0);

            // Ограничиваем сохраненное значение границами
            savedY = Mathf.Clamp(savedY, minYPosition, maxYPosition);

            // Применяем сохраненную позицию
            currentPosition = panelRectTransform.localPosition;
            currentPosition.y = savedY;
            panelRectTransform.localPosition = currentPosition;

            Debug.Log($"Позиция загружена: {savedY}");
        }
        else
        {
            // Если сохранения нет, используем текущую позицию
            currentPosition = panelRectTransform.localPosition;
        }
    }

    // Этот метод вызывается при прокрутке колесика мыши над объектом
    public void OnScroll(PointerEventData eventData)
    {
        // Получаем значение прокрутки колесика мыши
        // eventData.scrollDelta.y:
        //   > 0 - прокрутка вверх
        //   < 0 - прокрутка вниз
        float scrollDelta = eventData.scrollDelta.y;

        // Инвертируем направление если нужно
        if (invertScrollDirection)
        {
            scrollDelta = -scrollDelta;
        }

        // Вычисляем смещение
        float movement = scrollDelta * scrollSpeed;

        // Обновляем позицию
        currentPosition.y += movement;

        // Применяем ограничения
        currentPosition.y = Mathf.Clamp(currentPosition.y, minYPosition, maxYPosition);

        // Применяем новую позицию к панели
        panelRectTransform.localPosition = currentPosition;

        // Автоматически сохраняем при изменении
        if (savePosition)
        {
            SaveCurrentPosition();
        }

        // Выводим отладочную информацию
        Debug.Log($"Прокрутка: {scrollDelta}, Движение: {movement}, Новая позиция Y: {currentPosition.y}");
    }

    // Метод для сброса позиции (можно вызвать из других скриптов или кнопки UI)
    public void ResetPosition()
    {
        currentPosition.y = 0;
        currentPosition.y = Mathf.Clamp(currentPosition.y, minYPosition, maxYPosition);
        panelRectTransform.localPosition = currentPosition;

        // Сохраняем сброшенную позицию
        if (savePosition)
        {
            SaveCurrentPosition();
        }

        Debug.Log("Позиция панели сброшена");
    }

    // Метод для ручной установки позиции
    public void SetPosition(float newYPosition)
    {
        currentPosition.y = Mathf.Clamp(newYPosition, minYPosition, maxYPosition);
        panelRectTransform.localPosition = currentPosition;

        // Сохраняем установленную позицию
        if (savePosition)
        {
            SaveCurrentPosition();
        }
    }

    // Метод для получения текущей позиции
    public float GetCurrentPosition()
    {
        return currentPosition.y;
    }

    // Метод для получения нормализованной позиции (0-1)
    public float GetNormalizedPosition()
    {
        return Mathf.InverseLerp(minYPosition, maxYPosition, currentPosition.y);
    }

    // Метод для очистки сохраненной позиции
    public void ClearSavedPosition()
    {
        PlayerPrefs.DeleteKey(saveKey);
        PlayerPrefs.Save();
        Debug.Log($"Сохраненная позиция очищена для ключа: {saveKey}");
    }

    // Визуализация границ в редакторе (только в режиме Play)
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!showDebugBounds || !Application.isPlaying)
            return;
            
        if (panelRectTransform == null)
            return;
            
        // Получаем мировые координаты углов панели
        panelRectTransform.GetWorldCorners(worldCorners);
        
        // Рисуем границы панели
        Gizmos.color = Color.green;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(worldCorners[i], worldCorners[(i + 1) % 4]);
        }
        
        // Рисуем ограничивающие линии
        Vector3 center = panelRectTransform.position;
        float height = worldCorners[1].y - worldCorners[0].y;
        
        // Верхняя граница
        Vector3 topBoundary = center + Vector3.up * maxYPosition;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(topBoundary - Vector3.right * 200, topBoundary + Vector3.right * 200);
        
        // Нижняя граница
        Vector3 bottomBoundary = center + Vector3.up * minYPosition;
        Gizmos.DrawLine(bottomBoundary - Vector3.right * 200, bottomBoundary + Vector3.right * 200);
    }
#endif
}

// Вспомогательный класс для получения пути GameObject
public static class TransformExtensions
{
    public static string GetPath(this Transform current)
    {
        if (current.parent == null)
            return "/" + current.name;

        return current.parent.GetPath() + "/" + current.name;
    }
}