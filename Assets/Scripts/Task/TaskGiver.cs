using UnityEngine;

public class TaskGiver : MonoBehaviour
{
    [SerializeField] private GameTask taskToGive;
    private bool taskGiven = false; // prevent multiple triggers

    private void OnTriggerEnter(Collider other)
    {
        if (!taskGiven && other.CompareTag("Player"))
        {
            TaskManager.Instance.UnlockTask(taskToGive);
            taskGiven = true;

            // Optionally: show the Task Panel right away
            // TaskPanelUI.Instance.ShowPanel(); // if you have a singleton
        }
    }
}
