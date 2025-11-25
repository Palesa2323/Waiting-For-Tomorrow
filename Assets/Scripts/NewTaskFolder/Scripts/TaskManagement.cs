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

            // Pause Game
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

            // Populate UI text fields (using the formatted string logic)
            if (taskNameText != null)
                taskNameText.text = task.taskName;

            if (taskDescText != null)
                taskDescText.text = FormatTaskDescription(task);
        }
    }

    // Shortcut for dialogue system
    public void UnlockTask(TaskData task)
    {
        ShowTaskPanel(task);
    }

    public void AcceptTask()
    {
        if (currentTask == null)
        {
            Debug.LogWarning("No task assigned to accept! Cannot apply consequences.");
            return;
        }

        taskPanel.SetActive(false);

        // --- FIX 1: Use the dedicated GameManager method for full consequence handling ---
        // This method applies all rewards (money, stress, happiness, morality) 
        // AND performs the vital clamping/stress check.
        GameManager.Instance.ApplyTaskConsequences(currentTask);

        // --- FIX 2: Advance the Day and End the Player's Turn ---
        GameManager.Instance.NextDay();

        ResetTimeAndCursor();

        // Clear task
        currentTask = null;
    }
    private string FormatTaskDescription(TaskData task)
    {
        string description = task.taskDescription + "\n\n";

        // Money Reward
        description += $"<color=green>💰 REWARD: R {task.moneyReward:0.00}</color>\n";

        // Stress Change
        string stressColor = (task.stressChange > 0) ? "red" : "blue";
        string stressSign = (task.stressChange >= 0) ? "+" : "";
        description += $"<color={stressColor}>🧠 STRESS: {stressSign}{task.stressChange:0.0}</color>\n";

        // Happiness Change
        string happinessColor = (task.happinessReward > 0) ? "yellow" : "red";
        string happinessSign = (task.happinessReward >= 0) ? "+" : "";
        description += $"<color={happinessColor}>❤️ HAPPINESS: {happinessSign}{task.happinessReward:0.0}</color>\n";

        // Morality Change (Optional, but useful feedback)
        string moralColor = (task.moralScoreChange > 0) ? "red" : "green";
        string moralSign = (task.moralScoreChange >= 0) ? "+" : "";
        description += $"<color={moralColor}>⚖️ MORALITY: {moralSign}{task.moralScoreChange}</color>";

        return description;
    }

    public void DeclineTask()
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

        // --- FIX 3: Manually clamp the stats after the decline penalty ---
        GameManager.Instance.stress = Mathf.Clamp(GameManager.Instance.stress, 0f, 20f);
        GameManager.Instance.happiness = Mathf.Clamp(GameManager.Instance.happiness, 0f, 100f);

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
