using UnityEngine;
using System.Diagnostics;
using System.IO;

public class OpenDocument : MonoBehaviour
{
    public string fileName1 = "Конспект.docx";
    public string fileName2 = "Конспект полный.pdf";

    // Убрали метод Start() - он больше не нужен

    public void OpenFile1()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName1);

        if (File.Exists(filePath))
        {
            Process.Start(filePath);
            UnityEngine.Debug.Log("Файл открыт: " + filePath);
        }
        else
        {
            UnityEngine.Debug.LogError("Файл не найден! Путь: " + filePath);
            SearchForFile1();
        }
    }

    void SearchForFile1()
    {
        string[] possiblePaths = {
            Path.Combine(Directory.GetCurrentDirectory(), fileName1),
            Path.Combine(Application.dataPath, fileName1),
            Path.Combine(Application.dataPath, "StreamingAssets", fileName1)
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
    public void OpenFile2()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName2);

        if (File.Exists(filePath))
        {
            Process.Start(filePath);
            UnityEngine.Debug.Log("Файл открыт: " + filePath);
        }
        else
        {
            UnityEngine.Debug.LogError("Файл не найден! Путь: " + filePath);
            SearchForFile2();
        }
    }

    void SearchForFile2()
    {
        string[] possiblePaths = {
            Path.Combine(Directory.GetCurrentDirectory(), fileName2),
            Path.Combine(Application.dataPath, fileName2),
            Path.Combine(Application.dataPath, "StreamingAssets", fileName2)
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