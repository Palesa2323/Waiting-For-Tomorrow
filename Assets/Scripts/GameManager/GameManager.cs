using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 
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

    public float money = 50.00f;
    public float stress = 5f;
    public float happiness = 50f;
    public int currentDay = 1;
    public int maxDays = 7; // Goal to survive 7 days
    public int moralityScore = 0;
    public float foodAmount = 10f; // Starting Food amount

    [HideInInspector] public TaskData lastAcceptedTask;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public Slider stressSlider;
    public Slider happinessSlider;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI moralityText;

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
  
        moneyText.text = $"R {money:0.00}";
        if (foodText != null)
            foodText.text = $"Food: {foodAmount:0}";

        // Ensure sliders don't exceed max value (e.g., 100)
        stressSlider.value = Mathf.Clamp(stress, stressSlider.minValue, stressSlider.maxValue);
        happinessSlider.value = Mathf.Clamp(happiness, happinessSlider.minValue, happinessSlider.maxValue);

        if (moralityText != null)
        {
            string colorTag;
            string label;

            if (moralityScore > 5)
            {
                // High positive score = Unethical Path (Red/Orange warning)
                colorTag = "red";
                label = "Integrity Drained";
            }
            else if (moralityScore < -5)
            {
                // High negative score = Ethical Path (Green/Blue reward)
                colorTag = "green";
                label = "Integrity Strong";
            }
            else
            {
                // Neutral or early game (White/Yellow)
                colorTag = "yellow";
                label = "Morality Balance";
            }

            moralityText.text = $"<color={colorTag}>{label}</color>: {moralityScore}";
        }
    }

    public void CheckStressLevel()
    {
        // Check for immediate game over
        if (stress >= 15f)
        {
            Debug.Log("Game Over: Depression Hit!");
            if (SceneManager.GetActiveScene().name != loseSceneName)
                SceneManager.LoadScene(loseSceneName);
        }
    }

    public bool CheckWinCondition()
    {
        return currentDay > maxDays && money >= 100f && stress < 10f;
    }

    public void NextDay()
    {
        currentDay++;

        stress = Mathf.Max(0f, stress - 1.0f);

        Debug.Log($"--- Starting Day: {currentDay} ---");


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

  
        if (SceneManager.GetActiveScene().name != mainSceneName)
        {
            SceneManager.LoadScene(mainSceneName);
        }
        
    }

    // Change this line:
    // public void ApplyTaskRewards(TaskData task) // (Example of the likely old name)

    // TO THIS:
    // Inside GameManager.cs

    public void ApplyTaskConsequences(TaskData task)
    {
        if (task == null) return;

        money += task.moneyReward;
        stress += task.stressChange;
        happiness += task.happinessReward;
        moralityScore += task.moralScoreChange;

        // NEW: Apply food reward
        foodAmount += task.foodReward;

        // Clamp stats to valid ranges
        stress = Mathf.Clamp(stress, 0f, 20f);
        happiness = Mathf.Clamp(happiness, 0f, 100f);
        foodAmount = Mathf.Max(0f, foodAmount); // Food should not go below zero

        CheckStressLevel(); // Still checks for stress failure
                            // You might also add a CheckFoodLevel() if low food is a failure state
    }
}