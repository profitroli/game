using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToggleAnswer : MonoBehaviour
{
    public enum State { None, True, False }
    public State currentState = State.None;
    private TextMeshProUGUI btnText;
    private Image btnImage;

    void Awake()
    {
        btnText = GetComponentInChildren<TextMeshProUGUI>();
        btnImage = GetComponent<Image>();
        UpdateVisuals();
    }

    // Метод вызывается при нажатии на саму кнопку
    public void ToggleState()
    {
        if (currentState == State.None) currentState = State.True;
        else if (currentState == State.True) currentState = State.False;
        else currentState = State.None;

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        switch (currentState)
        {
            case State.None:
                btnText.text = "?";
                btnImage.color = Color.white;
                break;
            case State.True:
                btnText.text = "Правда";
                btnImage.color = Color.cyan; // Голубой при выборе
                break;
            case State.False:
                btnText.text = "Ложь";
                btnImage.color = Color.orange; // Желтый при выборе
                break;
        }
    }

    public void ResetButton()
    {
        currentState = State.None;
        UpdateVisuals();
    }
}