using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleManagerUI : MonoBehaviour
{
    public static PuzzleManagerUI Instance;

    [Header("Puzzle Pieces")]
    public List<PuzzlePieceUI> puzzlePieces = new List<PuzzlePieceUI>();
    public List<RectTransform> puzzleSlots = new List<RectTransform>(); // Слоты для кусочков

    [Header("UI Elements")]
    public Text progressText;
    public GameObject winPanel;
    public Button restartButton;

    [Header("Game Settings")]
    private int totalPieces;
    private int correctlyPlaced = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        totalPieces = puzzlePieces.Count;

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(ResetPuzzle);
        }

        // Случайным образом перемешиваем слоты (опционально)
        ShuffleSlots();

        UpdateProgress();
    }

    // Регистрация кусочков (вызывается из PuzzlePieceUI.Start)
    public void RegisterPiece(PuzzlePieceUI piece)
    {
        if (!puzzlePieces.Contains(piece))
        {
            puzzlePieces.Add(piece);
        }
    }

    public void PiecePlacedCorrectly()
    {
        correctlyPlaced++;
        UpdateProgress();

        if (correctlyPlaced >= totalPieces)
        {
            PuzzleCompleted();
        }
    }

    void UpdateProgress()
    {
        if (progressText != null)
        {
            progressText.text = $"Собрано: {correctlyPlaced}/{totalPieces}";
        }
    }

    void PuzzleCompleted()
    {
        Debug.Log("Пазл собран!");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Отключаем возможность перемещения всех кусочков
        foreach (var piece in puzzlePieces)
        {
            piece.canMove = false;
        }
    }

    public void ResetPuzzle()
    {
        correctlyPlaced = 0;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        // Сбрасываем все кусочки
        foreach (var piece in puzzlePieces)
        {
            piece.ResetPiece();
        }

        // Перемешиваем снова
        ShufflePieces();

        UpdateProgress();
    }

    // Метод для перемешивания позиций кусочков
    private void ShufflePieces()
    {
        List<Vector2> positions = new List<Vector2>();

        // Собираем все стартовые позиции
        foreach (var piece in puzzlePieces)
        {
            positions.Add(piece.GetComponent<RectTransform>().anchoredPosition);
        }

        // Перемешиваем позиции
        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 temp = positions[i];
            int randomIndex = Random.Range(i, positions.Count);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // Присваиваем новые позиции
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            puzzlePieces[i].GetComponent<RectTransform>().anchoredPosition = positions[i];
        }
    }

    // Метод для случайного назначения слотов (опционально)
    private void ShuffleSlots()
    {
        if (puzzleSlots.Count != puzzlePieces.Count) return;

        List<RectTransform> tempSlots = new List<RectTransform>(puzzleSlots);

        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            int randomIndex = Random.Range(0, tempSlots.Count);
            puzzlePieces[i].correctSlot = tempSlots[randomIndex];
            tempSlots.RemoveAt(randomIndex);
        }
    }
}