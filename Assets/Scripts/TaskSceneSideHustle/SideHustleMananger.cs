using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SideHustleManager : MonoBehaviour
{
    public TMP_Text taskNameText;
    public TMP_Text taskDescText;
    public Button acceptButton;
    public Button declineButton;

    private TaskData currentTask;

    void Start()
    {
        currentTask = GameManager.Instance.currentTask;

        if (currentTask != null)
        {
            taskNameText.text = currentTask.taskName;
            taskDescText.text = currentTask.taskDescription;
        }
        else
        {
            taskNameText.text = "No Task";
            taskDescText.text = "Nothing to do!";
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        acceptButton.onClick.AddListener(AcceptTask);
        declineButton.onClick.AddListener(DeclineTask);
    }

    void AcceptTask()
    {
        if (currentTask != null)
        {
            GameManager.Instance.money += currentTask.moneyReward;
            GameManager.Instance.happiness += currentTask.happinessReward;
            GameManager.Instance.stress += currentTask.stressChange;

            Debug.Log($"Accepted task: {currentTask.taskName}");
        }

        EndTask();
    }

    void DeclineTask()
    {
        if (currentTask != null)
        {
            GameManager.Instance.stress += currentTask.declineStressIncrease;
            GameManager.Instance.happiness -= currentTask.declineHappinessPenalty;

            Debug.Log($"Declined task: {currentTask.taskName}");
        }

        EndTask();
    }

    void EndTask()
    {
        GameManager.Instance.currentTask = null;

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene("GameScene"); // return to main scene
    }
}
