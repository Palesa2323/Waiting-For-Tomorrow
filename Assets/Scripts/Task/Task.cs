using UnityEngine;

[CreateAssetMenu(fileName = "NewGameTask", menuName = "Game/Task")]
public class GameTask : ScriptableObject
{
    public string taskName;
    public string npcName;
    public string topic;
    public bool isCompleted;
}

