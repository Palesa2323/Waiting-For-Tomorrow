using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskManagement : MonoBehaviour
{
    public static TaskManagement Instance;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("UI References")]
    public GameObject taskPanel;
    public TextMeshProUGUI taskNameText;
    public TextMeshProUGUI taskDescText;
    public Button acceptButton;
    public Button declineButton;

    [HideInInspector] public TaskData currentTask; // ScriptableObject task

    private void Start()
    {
        if (acceptButton != null)
            acceptButton.onClick.AddListener(AcceptTask);

        if (declineButton != null)
            declineButton.onClick.AddListener(DeclineTask);

        if (taskPanel != null)
            taskPanel.SetActive(false);
    }

    // Show the task panel with task details
    public void ShowTaskPanel(TaskData task)
    {
        if (task == null)
        {
            Debug.LogError("ShowTaskPanel called with null task!");
            return;
        }

        currentTask = task;

        if (taskPanel != null)
        {
            taskPanel.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f; // pause game

            if (taskNameText != null)
                taskNameText.text = task.taskName;

            if (taskDescText != null)
                taskDescText.text = task.taskDescription;

            Debug.Log($"Task Panel opened for: {task.taskName}");
        }
        else
        {
            Debug.LogError("Task Panel not assigned in TaskManagement!");
        }
    }

    // Shortcut for dialogue system
    public void UnlockTask(TaskData task)
    {
        ShowTaskPanel(task);
    }

    private void AcceptTask()
    {
        if (currentTask == null)
        {
            Debug.LogWarning("No task assigned to accept!");
            return;
        }

        taskPanel.SetActive(false);

        // Apply rewards
        GameManager.Instance.money += currentTask.moneyReward;
        GameManager.Instance.happiness += currentTask.happinessReward;
        GameManager.Instance.stress += currentTask.stressChange;

        ResetTimeAndCursor();

        // Load mini-game if needed
        if (currentTask.requiresMiniGame && !string.IsNullOrEmpty(currentTask.miniGameSceneName))
        {
            SceneManager.LoadScene(currentTask.miniGameSceneName);
        }
        else
        {
            // No mini-game → just advance day
            GameManager.Instance.NextDay();
        }

        // Clear task
        currentTask = null;
    }

    private void DeclineTask()
    {
        if (currentTask == null)
        {
            Debug.LogWarning("No task assigned to decline!");
            return;
        }

        taskPanel.SetActive(false);

        // Apply penalties
        GameManager.Instance.stress += currentTask.declineStressIncrease;
        GameManager.Instance.happiness += currentTask.declineHappinessPenalty;

        ResetTimeAndCursor();

        GameManager.Instance.NextDay();

        // Clear task
        currentTask = null;
    }

    private void ResetTimeAndCursor()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
