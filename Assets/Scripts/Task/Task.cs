using UnityEngine;

[CreateAssetMenu(fileName = "NewTask", menuName = "Tasks/Game Task")]
public class GameTask : ScriptableObject
{
    public string taskName;
    [TextArea] public string description;
    public int moneyReward;
    public int foodReward;
    public float stressChange;

    // NEW: metadata used to tie a Task to an NPC/topic
    [Header("Source (used by TaskManager to match)")]
    public string sourceNPC;   // e.g. "MamaZanele" - fill in inspector when you create the task asset
    public string topicId;     // must match DialogueTopic.id used in your dialogue system
}
