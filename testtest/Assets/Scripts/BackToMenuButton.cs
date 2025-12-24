using UnityEngine;
using UnityEngine.UI;

public class BackToMenuController : MonoBehaviour
{
    void Start()
    {
        Button backButton = GetComponent<Button>();
        if (backButton != null)
        {
            backButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void ReturnToMainMenu()
    {
        // Используем статический метод из MainMenuController
        MainMenuController.LoadMainMenuFromOtherScene();
    }

    void OnDestroy()
    {
        Button backButton = GetComponent<Button>();
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(ReturnToMainMenu);
        }
    }
}