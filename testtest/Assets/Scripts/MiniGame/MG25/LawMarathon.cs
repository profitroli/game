using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LawMarathon : MonoBehaviour
{
    [System.Serializable]
    public class LawFact
    {
        public string text;
        public bool isTrue;
    }

    public LawFact[] facts;
    public RectTransform card;
    public TextMeshProUGUI factText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText; // Текст "Молодец!" или "Не то..."
    public GameObject finishPanel;       // Окно финиша (панель)
    public TextMeshProUGUI resultText;   // Итоговый текст на панели

    public float speed = 250f;
    private int currentIndex = -1;
    private int score = 0;
    private bool isMoving = true;

    void Start()
    {
        ; // Прячем финиш в начале
        if (feedbackText != null) feedbackText.text = "Принимайте только верные утверждения";      // Очищаем подсказку
        PrepareNextFact();
    }

    void Update()
    {
        if (!isMoving || currentIndex >= facts.Length) return;

        card.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        if (card.anchoredPosition.x < -800f)
        {
            if (facts[currentIndex].isTrue)
            {
                score -= 5;
                ShowFeedback("Пропустил важное! Будь внимательнее.", Color.black);
            }
            PrepareNextFact();
        }
    }

    public void AcceptFact()
    {
        if (!isMoving) return;

        if (Mathf.Abs(card.anchoredPosition.x) < 150f)
        {
            if (facts[currentIndex].isTrue)
            {
                score += 10;
                ShowFeedback("Молодец! Правильно выбрал.", Color.green);
            }
            else
            {
                score -= 10;
                ShowFeedback("Не то! В следующий раз выбери правильно.", Color.red);
            }
            PrepareNextFact();
        }
    }

    // Метод для отображения короткой подсказки
    void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
            // Останавливаем старую корутину, если она шла, и запускаем новую
            StopAllCoroutines();
            StartCoroutine(ClearFeedback());
        }
    }

    IEnumerator ClearFeedback()
    {
        yield return new WaitForSeconds(3f);
        feedbackText.text = "";
    }

    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    void PrepareNextFact()
    {
        currentIndex++;
        if (currentIndex < facts.Length)
        {
            factText.text = facts[currentIndex].text;
            card.anchoredPosition = new Vector2(800f, -35.31f);
            scoreText.text = "Очки: " + score;
        }
        else
        {
            FinishGame();
        }
    }

    // Метод окончания игры
    void FinishGame()
    {
        isMoving = false;
        card.gameObject.SetActive(false);

        if (finishPanel != null)
        {
            finishPanel.SetActive(true); // Включаем окно финиша

            // Формируем похвалу в зависимости от очков
            string praise = score > 35 ? "Ты настоящий эксперт в праве!" : "Хороший результат, продолжай изучать!";
            resultText.text = $"ФИНИШ!\nТвой итог: {score} очков.\n{praise}";
        }
        else
        {
            // Если панели нет, выводим просто в текст карточки
            factText.text = $"ФИНИШ! Очки: {score}";
        }
    }
}