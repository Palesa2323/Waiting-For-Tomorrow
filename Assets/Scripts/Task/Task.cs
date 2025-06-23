using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Task")]
public class Task : ScriptableObject
{
    public string taskID;
    [TextArea] public string description;
    public bool isCompleted;
}
