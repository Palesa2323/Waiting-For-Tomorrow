using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Game Data/Task Data")]
public class TaskData : ScriptableObject
{
    [Header("Task Information")]
    public string taskName = "New Job Opportunity";
    [TextArea(3, 5)]
    public string taskDescription = "A brief description of what the job entails.";

    public float moneyReward;
    public float happinessReward;
    public float stressChange;

    public float declineHappinessPenalty;
    public float declineStressIncrease;

    public int moralScoreChange = 0;
}
