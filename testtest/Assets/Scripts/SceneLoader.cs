using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public static void LoadGamesScene()
    {
        LoadScene("GamesScene");
    }

    public static void LoadSettingsScene()
    {
        LoadScene("SettingsScene");
    }
}