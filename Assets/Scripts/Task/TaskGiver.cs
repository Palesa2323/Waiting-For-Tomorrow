using UnityEngine;

public class TaskGiver : MonoBehaviour
{
    [SerializeField] private GameTask taskToGive;

    public void GiveTask()
    {
        TaskManager.Instance.AddTask(taskToGive);
        Debug.Log($"Task given: {taskToGive.taskName}");
    }
}
