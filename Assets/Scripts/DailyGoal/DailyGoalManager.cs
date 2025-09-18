using UnityEngine;
using TMPro;

public class DailyGoalManager : MonoBehaviour
{
    public static DailyGoalManager Instance;

    [Header("UI")]
    public TMP_Text goalText;
    public TMP_Text rewardText; // optional: show reward

    [Header("Goals & Rewards")]
    public string[] dailyGoals;        // list of daily goals
    public int rewardAmount = 50;      // reward for completing goal
    public float stressReduction = 10f; // amount to reduce stress

    private int currentDayIndex = 0;
    private bool goalCompleted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateGoal();
    }

    // Call this every time a new day starts
    public void NextDay()
    {
        currentDayIndex++;
        if (currentDayIndex >= dailyGoals.Length)
            currentDayIndex = dailyGoals.Length - 1; // stay on last goal

        goalCompleted = false;
        UpdateGoal();
    }

    private void UpdateGoal()
    {
        if (goalText != null)
        {
            if (dailyGoals.Length > 0)
                goalText.text = "Daily Goal: " + dailyGoals[currentDayIndex];
            else
                goalText.text = "No goal today!";
        }

        if (rewardText != null)
            rewardText.text = "Reward: R" + rewardAmount;
    }

    // Call this when player completes the goal
    public void CompleteGoal()
    {
        if (goalCompleted) return; // prevent double rewards

        goalCompleted = true;

        // Reward player
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.ChangeMoney(rewardAmount);
            ResourceManager.Instance.ChangeStress(-stressReduction); // reduce stress
        }

        // Optional: visual feedback
        goalText.text = "✅ Goal Completed!";
    }
}
