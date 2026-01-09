using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FailPanelController : MonoBehaviour
{
    public TextMeshProUGUI failText;

    void Start()
    {
        failText.text = $"Неудача! Слишком много ошибочных решений.\nПопробуйте еще раз!";
    }

    public void OnRestartButton()
    {
        GameManager35.Instance.RestartGame();
    }
}