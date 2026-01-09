using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinPanelController : MonoBehaviour
{
    public TextMeshProUGUI winText;

    void Start()
    {
        winText.text = "Поздравляем! Так образовалась партия 'Белая Русь'!";
    }

    public void OnRestartButton()
    {
        GameManager35.Instance.RestartGame();
    }
}