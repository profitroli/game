using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LawMarathon : MonoBehaviour
{
    [System.Serializable]
    public class LawFact
    {
        public string text;
        public bool isTrue;
    }

    public LawFact[] facts; // Список фактов из лекции
    public RectTransform card; // Ссылка на объект карточки
    public TextMeshProUGUI factText; // Ссылка на текст внутри карточки
    public TextMeshProUGUI scoreText; // Ссылка на текст счета

    public float speed = 250f; // Скорость движения
    private int currentIndex = -1;
    private int score = 0;
    private bool isMoving = true;

    void Start()
    {
        PrepareNextFact();
    }

    void Update()
    {
        if (!isMoving || currentIndex >= facts.Length) return;

        // Двигаем карточку влево
        card.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        // Если карточка улетела совсем далеко влево (за экран)
        if (card.anchoredPosition.x < -800f)
        {
            // Если пропустили ПРАВИЛЬНЫЙ факт — это потеря очков
            if (facts[currentIndex].isTrue) score -= 5;
            PrepareNextFact();
        }
    }

    // Вызывается при нажатии на кнопку ПРИНЯТЬ
    public void AcceptFact()
    {
        if (!isMoving) return;

        // Проверяем, находится ли карточка в центре (X от -150 до 150)
        if (Mathf.Abs(card.anchoredPosition.x) < 150f)
        {
            if (facts[currentIndex].isTrue)
            {
                score += 10;
                Debug.Log("Верно!");
            }
            else
            {
                score -= 10;
                Debug.Log("Это ложь!");
            }
            PrepareNextFact();
        }
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
            // Возвращаем карточку за правую границу экрана (например, X = 800)
            card.anchoredPosition = new Vector2(800f, 0);
            scoreText.text = "Очки: " + score;
        }
        else
        {
            isMoving = false;
            card.gameObject.SetActive(false);
            factText.text = "ФИНИШ! Ваш счет: " + score;
        }
    }
}