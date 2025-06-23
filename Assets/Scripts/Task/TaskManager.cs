using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<Task> allTasks = new List<Task>();
    public TaskUI taskUI;

    private bool isTaskUIActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Example: Toggle task UI with T key (change this to your actual UI toggle)
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTaskUIActive = !isTaskUIActive;
            taskUI.gameObject.SetActive(isTaskUIActive);

            if (isTaskUIActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
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
