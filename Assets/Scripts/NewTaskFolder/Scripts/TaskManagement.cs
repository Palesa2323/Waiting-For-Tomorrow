using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManagement : MonoBehaviour
{
    public static TaskManagement Instance;
    void Awake() { Instance = this; }

    [Header("UI References")]
    public GameObject taskPanel;
    public TextMeshProUGUI taskNameText;
    public TextMeshProUGUI taskDescText;
    public Button acceptButton;
    public Button declineButton;

    private TaskData currentTask; // Store the task being processed

    public void ShowTaskPanel(TaskData task)
    {
        if (GameManager.Instance.currentDay > GameManager.Instance.maxDays) return; // Don't show if game is over

        currentTask = task; // Store the passed-in task
        taskPanel.SetActive(true);

        // Populate UI with data from the ScriptableObject
        taskNameText.text = task.taskName;
        taskDescText.text = task.taskDescription;

        // Set up button listeners dynamically
        acceptButton.onClick.RemoveAllListeners(); // Clear old listeners
        declineButton.onClick.RemoveAllListeners();

        acceptButton.onClick.AddListener(AcceptTask);
        declineButton.onClick.AddListener(DeclineTask);
    }

    // --- BUTTON ACTIONS ---

    private void AcceptTask()
    {
        taskPanel.SetActive(false);

        // 1. Apply Rewards/Consequences
        GameManager.Instance.money += currentTask.moneyReward;
        GameManager.Instance.happiness += currentTask.happinessReward;
        GameManager.Instance.stress += currentTask.stressChange;

        // 2. Load Mini-Game or Advance Day
        if (currentTask.requiresMiniGame)
        {
            // Load the mini-game scene for the task
            SceneManager.LoadScene(currentTask.miniGameSceneName);
        }
        else
        {
            // Task is instant (like 'Rest' or 'Talk'), advance the day after completion
            GameManager.Instance.NextDay();
        }
        // *The reward for a mini-game task will be applied upon exiting the mini-game scene.*
    }

    private void DeclineTask()
    {
        taskPanel.SetActive(false);

        // Apply Penalties (The Goal is to increase stress and decrease happiness)
        GameManager.Instance.stress += currentTask.declineStressIncrease;
        GameManager.Instance.happiness += currentTask.declineHappinessPenalty;

        // Advance the day immediately after declining a task
        GameManager.Instance.NextDay();
    }
}
