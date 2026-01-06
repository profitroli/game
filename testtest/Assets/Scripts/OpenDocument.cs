using UnityEngine;
using System.Diagnostics;
using System.IO;

public class OpenDocument : MonoBehaviour
{
    public string fileName = "Конспект.docx";

    // Убрали метод Start() - он больше не нужен

    public void OpenFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            Process.Start(filePath);
            UnityEngine.Debug.Log("Файл открыт: " + filePath);
        }
        else
        {
            UnityEngine.Debug.LogError("Файл не найден! Путь: " + filePath);
            SearchForFile();
        }
    }

    void SearchForFile()
    {
        string[] possiblePaths = {
            Path.Combine(Directory.GetCurrentDirectory(), fileName),
            Path.Combine(Application.dataPath, fileName),
            Path.Combine(Application.dataPath, "StreamingAssets", fileName)
        };

        foreach (string path in possiblePaths)
        {
            if (File.Exists(path))
            {
                Process.Start(path);
                UnityEngine.Debug.Log("Файл найден и открыт: " + path);
                return;
            }
        }

        UnityEngine.Debug.LogError("Файл не найден ни в одном из возможных мест!");
    }
}