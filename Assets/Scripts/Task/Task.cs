using UnityEngine;

[CreateAssetMenu(fileName = "NewTask", menuName = "Tasks/Game Task")]
public class GameTask : ScriptableObject
{
    public string taskName;
    public string npcName;
    public string topic;
    public bool isCompleted;
}
