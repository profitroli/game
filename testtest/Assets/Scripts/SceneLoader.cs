using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Основные сцены приложения
    public const string MAIN_MENU = "MainMenu";
    public const string LECTURES_SCENE = "LecturesScene";
    public const string GAMES_SCENE = "GamesScene";
    public const string PREPARATION_SCENE = "PreparationScene";
    public const string SETTINGS_SCENE = "SettingsScene";

    // Основной метод для загрузки сцен
    public static void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Имя сцены не может быть пустым!");
            return;
        }

        // Проверяем существование сцены
        if (!IsSceneInBuildSettings(sceneName))
        {
            Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings!");
            LoadMainMenu(); // Возвращаем в главное меню при ошибке
            return;
        }

        // Загружаем сцену
        SceneManager.LoadScene(sceneName);
    }

    // Методы для быстрой загрузки конкретных сцен
    public static void LoadMainMenu()
    {
        LoadScene(MAIN_MENU);
    }

    public static void LoadLecturesScene()
    {
        LoadScene(LECTURES_SCENE);
    }

    public static void LoadGamesScene()
    {
        LoadScene(GAMES_SCENE);
    }

    public static void LoadPreparationScene()
    {
        LoadScene(PREPARATION_SCENE);
    }

    public static void LoadSettingsScene()
    {
        LoadScene(SETTINGS_SCENE);
    }

    // Метод для проверки наличия сцены в Build Settings
    private static bool IsSceneInBuildSettings(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            // Получаем путь к сцене и извлекаем имя
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string nameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (nameFromPath == sceneName)
            {
                return true;
            }
        }

        return false;
    }
    // Дополнительные полезные методы
    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void ReloadCurrentScene()
    {
        string currentScene = GetCurrentSceneName();
        LoadScene(currentScene);
    }

    public static void LoadSceneAsync(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Имя сцены не может быть пустым!");
            return;
        }

        if (!IsSceneInBuildSettings(sceneName))
        {
            Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings!");
            return;
        }

        SceneManager.LoadSceneAsync(sceneName);
    }
}