using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FamilyTreeManager : MonoBehaviour
{
    [System.Serializable]
    public class Member
    {
        public DraggableCard2 card; // Фото князя
        public int correctSlotID;  // Куда его нужно поставить
    }

    public Member[] familyMembers;
    public TextMeshProUGUI statusText;

    public void CheckTree()
    {
        int correct = 0;
        foreach (var m in familyMembers)
        {
            if (m.card.currentSlotID == m.correctSlotID) correct++;
        }

        if (correct == familyMembers.Length)
        {
            statusText.text = "Семья воссоединена! Верно.";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "Кто-то не на своём месте...";
            statusText.color = Color.red;
        }
    }
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetGame()
    {
        foreach (var m in familyMembers) m.card.ResetPosition();
        statusText.text = "Расставьте правильно родственные связи ";
        statusText.color = Color.black;
    }
}