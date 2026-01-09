using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class Choice
{
    public string choiceAText;
    public string choiceBText;
    public int choiceAImpact = 1;
    public int choiceBImpact = -1;
}

[System.Serializable]
public class Stage
{
    public string stageName;
    [TextArea(2, 4)]
    public string stageIntroText; // Текст, который появляется в начале этапа
    public Sprite characterSprite;
    public Choice[] choices;
}

public class GameManager35 : MonoBehaviour
{
    public static GameManager35 Instance;

    public Stage[] stages;

    // Ссылки на UI элементы
    public Image characterImage;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI stageIntroText; // Текстовое поле для вступительного текста этапа
    public TextMeshProUGUI feedbackText; // Поле для обратной связи
    public Button choiceButtonA;
    public Button choiceButtonB;
    public GameObject winPanel;
    public GameObject failPanel;

    private int currentStageIndex = 0;
    private int currentChoiceIndex = 0;
    private int stageProgress = 0;
    private int badChoicesCount = 0;
    private const int MAX_BAD_CHOICES = 5;
    private const int REQUIRED_PROGRESS = 4;

    // Цвета для обратной связи
    private Color goodColor = Color.green;
    public Color badColor = Color.red;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        choiceButtonA.onClick.AddListener(() => MakeChoice(0));
        choiceButtonB.onClick.AddListener(() => MakeChoice(1));

        // Инициализируем тексты
        feedbackText.text = "";
        stageIntroText.text = "";

        LoadCurrentStage();
    }

    void LoadCurrentStage()
    {
        if (currentStageIndex >= stages.Length)
        {
            ShowWinScreen();
            return;
        }

        var stage = stages[currentStageIndex];

        // Обновляем UI
        characterImage.sprite = stage.characterSprite;
        stageText.text = $"Этап {currentStageIndex + 1}: {stage.stageName}";
        progressText.text = $"Прогресс этапа: {stageProgress}/{REQUIRED_PROGRESS}";

        // Показываем вступительный текст этапа
        ShowStageIntro(stage.stageIntroText);

        // Загружаем текущий выбор
        LoadCurrentChoice();
    }

    void ShowStageIntro(string introText)
    {
        stageIntroText.text = introText;
        stageIntroText.color = Color.blue; // Синий цвет для вступительного текста

        // Автоматически скрываем через 3 секунды
        StartCoroutine(HideStageIntroAfterDelay(3f));
    }

    IEnumerator HideStageIntroAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Плавно скрываем текст
        float fadeDuration = 0.2f;
        float elapsedTime = 0f;
        Color startColor = stageIntroText.color;
        Color transparentColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            stageIntroText.color = Color.Lerp(startColor, transparentColor, elapsedTime / fadeDuration);
            yield return null;
        }

        stageIntroText.text = "";
    }

    void LoadCurrentChoice()
    {
        var stage = stages[currentStageIndex];

        if (currentChoiceIndex >= stage.choices.Length)
            currentChoiceIndex = stage.choices.Length - 1;

        var choice = stage.choices[currentChoiceIndex];

        var textA = choiceButtonA.GetComponentInChildren<TextMeshProUGUI>();
        var textB = choiceButtonB.GetComponentInChildren<TextMeshProUGUI>();
        textA.text = choice.choiceAText;
        textB.text = choice.choiceBText;
    }

    void MakeChoice(int choiceType)
    {
        var stage = stages[currentStageIndex];
        var choice = stage.choices[currentChoiceIndex];

        int impact = (choiceType == 0) ? choice.choiceAImpact : choice.choiceBImpact;

        if (impact < 0) // Плохой выбор
        {
            ShowFeedback("Неверно!", badColor);
            badChoicesCount++;
            if (badChoicesCount >= MAX_BAD_CHOICES)
            {
                ShowFailScreen();
                return;
            }

            // Откат на предыдущий выбор
            if (currentChoiceIndex > 0)
                currentChoiceIndex--;

            // Отнимаем прогресс
            stageProgress = Mathf.Max(0, stageProgress - 1);
        }
        else // Хороший выбор
        {
            stageProgress += impact;

            // Переходим к следующему выбору
            if (currentChoiceIndex < stage.choices.Length - 1)
                currentChoiceIndex++;

            // Проверяем завершение этапа
            if (stageProgress >= REQUIRED_PROGRESS)
            {
                // Переходим на следующий этап
                currentStageIndex++;
                currentChoiceIndex = 0;
                stageProgress = 0;
                badChoicesCount = 0;

                LoadCurrentStage();
                return;
            }
        }

        progressText.text = $"Прогресс этапа: {stageProgress}/{REQUIRED_PROGRESS}";
        LoadCurrentChoice();
    }

    void ShowFeedback(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;

        StartCoroutine(HideFeedbackAfterDelay(2f));
    }

    IEnumerator HideFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float fadeDuration = 0.5f;
        float elapsedTime = 0f;
        Color startColor = feedbackText.color;
        Color transparentColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            feedbackText.color = Color.Lerp(startColor, transparentColor, elapsedTime / fadeDuration);
            yield return null;
        }

        feedbackText.text = "";
    }

    void ShowWinScreen()
    {
        winPanel.SetActive(true);
        choiceButtonA.gameObject.SetActive(false);
        choiceButtonB.gameObject.SetActive(false);
    }

    void ShowFailScreen()
    {
        failPanel.SetActive(true);
        choiceButtonA.gameObject.SetActive(false);
        choiceButtonB.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        currentStageIndex = 0;
        currentChoiceIndex = 0;
        stageProgress = 0;
        badChoicesCount = 0;

        winPanel.SetActive(false);
        failPanel.SetActive(false);
        choiceButtonA.gameObject.SetActive(true);
        choiceButtonB.gameObject.SetActive(true);

        feedbackText.text = "";
        stageIntroText.text = "";

        LoadCurrentStage();
    }
}