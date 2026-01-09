using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePieceUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Piece Settings")]
    public int pieceID;
    public RectTransform correctSlot; // Слот, куда нужно поместить кусочек
    public float snapDistance = 30f;
    public bool canMove = true;

    [Header("References")]
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Image pieceImage;
    private Vector2 originalPosition;
    private bool isCorrectlyPlaced = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        pieceImage = GetComponent<Image>();

        originalPosition = rectTransform.anchoredPosition;

        // Регистрируем кусочек в менеджере
        if (PuzzleManagerUI.Instance != null)
        {
            PuzzleManagerUI.Instance.RegisterPiece(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canMove || isCorrectlyPlaced) return;

        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        // Поднимаем на верхний слой
        transform.SetAsLastSibling();

        // Визуальный эффект
        pieceImage.color = new Color(1f, 1f, 1f, 0.8f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canMove || isCorrectlyPlaced) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canMove) return;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        CheckPosition();
    }

    private void CheckPosition()
    {
        if (correctSlot == null) return;

        float distance = Vector2.Distance(rectTransform.anchoredPosition, correctSlot.anchoredPosition);

        if (distance <= snapDistance)
        {
            // Снаппим к правильной позиции
            rectTransform.anchoredPosition = correctSlot.anchoredPosition;

            if (!isCorrectlyPlaced)
            {
                isCorrectlyPlaced = true;
                canMove = false;

                // Визуальная обратная связь
                pieceImage.color = Color.white;

                // Уведомляем менеджер
                PuzzleManagerUI.Instance.PiecePlacedCorrectly();
            }
        }
        else
        {
            pieceImage.color = Color.white;
        }
    }

    public void ResetPiece()
    {
        rectTransform.anchoredPosition = originalPosition;
        isCorrectlyPlaced = false;
        canMove = true;
        canvasGroup.blocksRaycasts = true;
        pieceImage.color = Color.white;
    }
}