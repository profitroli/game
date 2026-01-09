using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CandidateCheck : MonoBehaviour
{
    [System.Serializable]
    public class CandidateProfile
    {
        public string name;                  // ОТДЕЛЬНО ИМЯ
        public string age;                   // ОТДЕЛЬНО ВОЗРАСТ
        [TextArea] public string otherInfo;  // ОСТАЛЬНАЯ БИОГРАФИЯ (место рождения, стаж)
        public Sprite photo;
        public bool isEligible;
    }

    [Header("Данные игры")]
    public CandidateProfile[] profiles;
    private int currentIndex = 0;
    private int score = 0;

    [Header("Ссылки на UI")]
    public RectTransform candidateCard;
    public Image candidatePhotoDisplay;
    public TextMeshProUGUI nameText;          // Ссылка на текст Имени
    public TextMeshProUGUI ageText;           // Ссылка на текст Возраста
    public TextMeshProUGUI bioText;           // Ссылка на текст Биографии
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI scoreText;
    public GameObject finishPanel;
    public TextMeshProUGUI resultLabel;

    [Header("Параметры анимации")]
    public float fallSpeed = 1200f;
    public float targetYPosition = 0f;
    private bool isAnimating = false;

    void Start()
    {
        if (finishPanel != null) finishPanel.SetActive(false);
        candidateCard.anchoredPosition = new Vector2(0, 800);
        ShowCandidate();
    }

    void Update()
    {
        if (isAnimating)
        {
            candidateCard.anchoredPosition = Vector2.MoveTowards(
                candidateCard.anchoredPosition,
                new Vector2(0, targetYPosition),
                fallSpeed * Time.deltaTime
            );

            if (candidateCard.anchoredPosition.y == targetYPosition) isAnimating = false;
        }
    }

    public void MakeDecision(bool playerApproved)
    {
        if (isAnimating || currentIndex >= profiles.Length) return;

        if (playerApproved == profiles[currentIndex].isEligible)
        {
            score += 10;
            StartCoroutine(ShowFeedback("Верно!", Color.green));
        }
        else
        {
            score -= 5;
            StartCoroutine(ShowFeedback("Ошибка!", Color.red));
        }

        currentIndex++;
        StartCoroutine(HideAndShowNextCandidate());
    }

    IEnumerator HideAndShowNextCandidate()
    {
        // Улетает вниз
        float elapsed = 0f;
        Vector2 startPos = candidateCard.anchoredPosition;
        Vector2 endPos = new Vector2(0, -1200);
        while (elapsed < 0.2f)
        {
            candidateCard.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / 0.4f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ShowCandidate();
        yield return new WaitForSeconds(0.1f);

        // Падает сверху
        candidateCard.anchoredPosition = new Vector2(0, 1200);
        isAnimating = true;
    }

    void ShowCandidate()
    {
        if (currentIndex < profiles.Length)
        {
            // Распределяем данные по разным текстовым полям
            nameText.text = profiles[currentIndex].name;
            ageText.text = profiles[currentIndex].age;
            bioText.text = profiles[currentIndex].otherInfo;

            if (profiles[currentIndex].photo != null)
                candidatePhotoDisplay.sprite = profiles[currentIndex].photo;

            scoreText.text = "Очки: " + score;
            isAnimating = true;
        }
        else
        {
            ShowFinalResult();
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void ShowFinalResult()
    {
        candidateCard.gameObject.SetActive(false);
        finishPanel.SetActive(true);
        string praise = (score >= 30) ? "Вы отлично знаете требования к кандидату!" : "Попробуйте еще раз!";
        resultLabel.text = $"ФИНИШ!\nОчки: {score}\n{praise}";
    }

    IEnumerator ShowFeedback(string msg, Color col)
    {
        feedbackText.text = msg;
        feedbackText.color = col;
        yield return new WaitForSeconds(0.8f);
        feedbackText.text = "Определите кто может стать президентом";
        feedbackText.color = Color.black;
    }

    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}