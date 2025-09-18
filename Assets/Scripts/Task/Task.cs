using UnityEngine;

[CreateAssetMenu(fileName = "NewTask", menuName = "Tasks/Game Task")]
public class GameTask : ScriptableObject
{
    [Header("Task Info")]
    public string taskName;
    [TextArea] public string description;
    public int moneyReward;
    public int foodReward;
    public float stressChange;

    [Header("Source (used by TaskManager to match)")]
    public string sourceNPC;   // e.g. "MamaZanele" - set in inspector or from dialogue
    public string topicId;     // matches DialogueTopic.id

    [Header("Optional MiniGame")]
    public string miniGameScene; // leave empty if no scene
}

