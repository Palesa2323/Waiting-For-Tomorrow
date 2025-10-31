using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class GameManager : MonoBehaviour
{
    // 1. Singleton Pattern (Ensures only one GameManager exists)
    public static GameManager Instance;
    void Awake() { Instance = this; }

    // 2. Core Stats (Public so other scripts can access them)
    public float money = 50f;
    public float stress = 5f;
    public float happiness = 50f;
    public int currentDay = 1;
    public int maxDays = 7; // Goal to survive 7 days

    [Header("UI References")]
    public Slider moneySlider;
    public Slider stressSlider;
    public Slider happinessSlider;

    void Update()
    {
        // Update the UI Sliders every frame
        UpdateHUD();

        // Core Logic Check
        CheckStressLevel();
    }

    public void UpdateHUD()
    {
        // Set slider values (Assuming Max Value is 100 for now)
        moneySlider.value = money;
        stressSlider.value = stress;
        happinessSlider.value = happiness;
    }

    // The core consequence check!
    public void CheckStressLevel()
    {
        if (stress >= 20f)
        {
            // This will be called when the player falls into depression
            // For now, print a message. Later, we load the LoseScene.
            Debug.Log("Game Over: Depression Hit!");
        }
    }

    public void NextDay()
    {
        currentDay++;
        if (currentDay > maxDays)
        {
            // Win/Lose check goes here
        }
        // Add scene fade logic here later
    }
}