using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Task/Basic Task")]
public class Task : ScriptableObject
{
    public string taskName;
    [TextArea(2, 4)] public string description;

    public bool isCompleted;

    // Optional: a target the player should go to
    public Transform taskLocation;
}
