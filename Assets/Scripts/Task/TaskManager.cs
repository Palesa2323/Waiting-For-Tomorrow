using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public List<Task> allTasks = new List<Task>();
    public TaskUI taskUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void UnlockTask(Task task)
    {
        if (!allTasks.Contains(task))
        {
            allTasks.Add(task);
            taskUI.UpdateTaskList(allTasks);
        }
    }

    public void CompleteTask(Task task)
    {
        task.isCompleted = true;
        taskUI.UpdateTaskList(allTasks);
    }
}
