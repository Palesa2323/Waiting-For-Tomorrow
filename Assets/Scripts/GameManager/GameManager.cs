using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Added for later scene loading

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum GameState { WAKE_UP, CHOOSE_ACTIONS, CONSEQUENCES, NEXT_DAY, GAME_OVER }
    public GameState currentState = GameState.WAKE_UP;

    public float money = 50.00f; 
    public float stress = 5f;
    public float happiness = 50f;
    public int currentDay = 1;
    public int maxDays = 7; // Goal to survive 7 days

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public Slider stressSlider;
    public Slider happinessSlider;

    
    [Header("Game State")]
    public string loseSceneName = "LoseScene";

    void Update()
    {
     
        UpdateHUD();

        CheckStressLevel();
    }

    public void UpdateHUD()
    {

        moneyText.text = $"R {money:0.00}";

        stressSlider.value = stress;
        happinessSlider.value = happiness;
    }

    public void CheckStressLevel()
    {
        if (stress >= 20f && !IsGameOver())
        {
            Debug.Log("Game Over: Depression Hit!");
        }
    }

    public bool IsGameOver()
    {
        return stress >= 20f || (currentDay > maxDays && !CheckWinCondition());
    }

    public bool CheckWinCondition()
    {
        return currentDay > maxDays && money >= 150f && stress < 10f;
    }

    public void NextDay()
    {
        currentDay++;
        Debug.Log($"Starting Day: {currentDay}"); // Feedback

        if (currentDay > maxDays)
        {
            // End of the game progression check
            if (CheckWinCondition())
            {
                Debug.Log("Game Over: VICTORY!");
                // SceneManager.LoadScene("WinScene"); 
            }
            else
            {
                Debug.Log("Game Over: Time Limit Reached!");
            }
        }
        // Add scene fade logic here later
    }
}