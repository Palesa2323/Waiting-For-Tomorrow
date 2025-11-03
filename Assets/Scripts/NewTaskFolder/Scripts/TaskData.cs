using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Game Data/Task Data")]
public class TaskData : ScriptableObject
{
    [Header("Task Information")]
    public string taskName = "New Job Opportunity";
    [TextArea(3, 5)]
    public string taskDescription = "A brief description of what the job entails.";

    [Header("Consequences (Accept)")]
    // We'll use negative values for penalties later
    public float moneyReward = 20.00f;
    public float happinessReward = 5f;
    public float stressChange = -3f; // Stress decreases on successful completion
    public bool requiresMiniGame = true; // True if a separate mini-game scene is loaded
    public string miniGameSceneName = "SideHustleScene"; // Scene to load

    [Header("Consequences (Decline)")]
    public float declineStressIncrease = 5f;
    public float declineHappinessPenalty = -5f;
}
