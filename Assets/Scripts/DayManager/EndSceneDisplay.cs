using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndSceneDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;

    [Header("Scene Settings")]
    public bool isWinScene = false;
    public string mainMenuSceneName = "MainMenuScene";

    void Start()
    {
        // 1. Check if GameManager exists (it should, as it persists)
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found. Cannot display final results!");
            headerText.text = "ERROR: Game data lost.";
            bodyText.text = "Please ensure the GameManager script persists across scenes.";
            return;
        }

        if (isWinScene)
        {
            DisplayWinOutcome();
        }
        else
        {
            DisplayLoseOutcome();
        }
    }

    // --- Win Logic ---

    private void DisplayWinOutcome()
    {
        // Get the final values from the persistent GameManager
        int morality = GameManager.Instance.moralityScore;
        float finalStress = GameManager.Instance.stress;
        float finalHappiness = GameManager.Instance.happiness;
        float finalMoney = GameManager.Instance.money;

        string header = "The Crisis Averted";
        string body = "";

        if (morality > 15)
        {
            header = "The Dark Victory 😈";
            body = "The bond is paid, but the methods have changed you. Your community whispers of the harm left in your wake. You survived, but you now walk a darker, solitary path. You gained freedom but lost your soul.";
        }
        else if (morality < -15)
        {
            header = "The Moral Triumph 🙏";
            body = "You proved that integrity is a wealth of its own. You scraped by honestly, prioritizing character over quick wealth. Your future is built on virtue, despite the financial strain you endured.";
        }
        else
        {
            header = "The Hard-Won Balance ⚖️";
            body = "You navigated the tightrope, balancing risk and ethics. You secured the funds and held onto your humanity. You are exhausted, but you are whole, with the respect of your community.";
        }

        // Append final stats to the body text
        body += $"\n\n--- Final Stats ---\nMoney: R{finalMoney:0.00}\nStress: {finalStress:0.0}/20\nHappiness: {finalHappiness:0.0}%";

        headerText.text = header;
        bodyText.text = body;
    }

    // --- Lose Logic ---

    private void DisplayLoseOutcome()
    {
        // Check which condition caused the loss
        bool stressFailed = GameManager.Instance.stress >= 20f;
        float finalMoney = GameManager.Instance.money;

        string header = "The Failure";
        string body = "";

        if (stressFailed)
        {
            header = "Mental Collapse 🧠";
            body = "You ran too fast, took too much risk, and ignored your own mental health. The stress threshold was crossed, and your mind collapsed. The debt is secondary; you lost your sanity.";
        }
        else // Financial/Time Failure
        {
            header = "Time Ran Out ⏳";
            body = $"The clock ran out. Despite your efforts, you failed to secure the necessary R150 bond. You only managed R{finalMoney:0.00}. The consequences of the financial crisis are upon you.";
        }

        // Append final stats
        body += $"\n\n--- Final Stats ---\nMoney: R{finalMoney:0.00}\nStress: {GameManager.Instance.stress:0.0}/20\nHappiness: {GameManager.Instance.happiness:0.0}%";

        headerText.text = header;
        bodyText.text = body;
    }

    public void RestartGame()
    {
        // 1. Check if the instance exists before trying to access its properties
        if (GameManager.Instance != null)
        {
            // 2. Destroy the persistent GameManager GameObject to prevent duplicates
            // This is CRUCIAL for a clean restart.
            Destroy(GameManager.Instance.gameObject);
        }

        // 3. Load the Main Menu scene using the public field
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("Main Menu Scene Name is not set in the Inspector! Cannot restart game.");
        }
    }
}