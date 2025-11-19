using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Added for scene loading

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public float money = 50.00f; // NEW: The money variable was missing
    public float stress = 5f;
    public float happiness = 50f;
    public int currentDay = 1;
    public int maxDays = 7; // Goal to survive 7 days

    
    [HideInInspector] public TaskData lastAcceptedTask;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public Slider stressSlider;
    public Slider happinessSlider;

    [Header("Scene Names")]
    public string loseSceneName = "LoseScene";
    public string winSceneName = "WinScene";
    public string mainSceneName = "MainTownshipScene";

    void Update()
    {
        UpdateHUD();
        CheckStressLevel();
    }

    public void UpdateHUD()
    {
        // FIX: Display the actual 'money' variable formatted as currency.
        moneyText.text = $"R {money:0.00}";

        // Ensure sliders don't exceed max value (e.g., 100)
        stressSlider.value = Mathf.Clamp(stress, stressSlider.minValue, stressSlider.maxValue);
        happinessSlider.value = Mathf.Clamp(happiness, happinessSlider.minValue, happinessSlider.maxValue);
    }

    public void CheckStressLevel()
    {
        // Check for immediate game over
        if (stress >= 20f)
        {
            Debug.Log("Game Over: Depression Hit!");
            if (SceneManager.GetActiveScene().name != loseSceneName)
                SceneManager.LoadScene(loseSceneName);
        }
    }

    public bool CheckWinCondition()
    {
        // Example: Win if you survived 7 days AND have decent money/low stress
        return currentDay > maxDays && money >= 150f && stress < 10f;
    }

    public void NextDay()
    {
        currentDay++;

        // Optional: Apply a small daily stress reduction for sleeping
        stress = Mathf.Max(0f, stress - 1.0f);

        Debug.Log($"--- Starting Day: {currentDay} ---");

        // 1. End Game Check
        if (currentDay > maxDays)
        {
            if (CheckWinCondition())
            {
                Debug.Log("Game Over: VICTORY!");
                SceneManager.LoadScene(winSceneName);
            }
            else
            {
                Debug.Log("Game Over: Time Limit Reached/Failed to Meet Goals!");
                SceneManager.LoadScene(loseSceneName);
            }
            return;
        }

        // 2. Load Main Scene if coming back from Mini-Game
        if (SceneManager.GetActiveScene().name != mainSceneName)
        {
            SceneManager.LoadScene(mainSceneName);
        }

        // 3. Reset NPC Interaction State (Optional but necessary for a clean loop)
        // You would need a list of NPCTrigger objects and loop through them to call ResetForNewDay().
    }

    public void ApplyTaskRewards(TaskData task)
    {
        if (task == null) return;

        money += task.moneyReward;
        stress += task.stressChange;
        happiness += task.happinessReward;

        lastAcceptedTask = null; // Clear the task reference
        CheckStressLevel(); // Re-check stress immediately
    }
}