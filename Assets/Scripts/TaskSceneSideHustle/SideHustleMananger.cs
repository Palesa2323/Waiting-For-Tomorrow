using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SideHustleManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Button acceptButton;
    public Button declineButton;

    private TaskData currentTask;

    void Start()
    {
        currentTask = GameManager.Instance.currentTask;

        if (currentTask == null)
        {
            Debug.LogError("No task assigned! Returning to GameScene.");
            SceneManager.LoadScene("GameScene");
            return;
        }

        // Set UI
        nameText.text = currentTask.taskName;
        descriptionText.text = currentTask.taskDescription;

        // Button listeners
        acceptButton.onClick.AddListener(AcceptTask);
        declineButton.onClick.AddListener(DeclineTask);

        // Enable cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    void AcceptTask()
    {
        GameManager.Instance.money += currentTask.moneyReward;
        GameManager.Instance.happiness += currentTask.happinessReward;
        GameManager.Instance.stress += currentTask.stressChange;

        // If mini-game exists
        if (currentTask.requiresMiniGame)
        {
            SceneManager.LoadScene(currentTask.miniGameSceneName);
        }
        else
        {
            ReturnToGameScene();
        }
    }

    void DeclineTask()
    {
        GameManager.Instance.happiness -= currentTask.declineHappinessPenalty;
        GameManager.Instance.stress += currentTask.declineStressIncrease;

        ReturnToGameScene();
    }

    void ReturnToGameScene()
    {
        GameManager.Instance.currentTask = null;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene("GameScene");
    }
}
