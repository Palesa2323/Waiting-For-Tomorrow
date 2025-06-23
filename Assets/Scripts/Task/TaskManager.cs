using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<GameTask> allTasks = new List<GameTask>();
    public TaskUI taskUI;

    private bool isTaskUIActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Toggle task UI with T key (adjust as needed)
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

    public void UnlockTask(GameTask task)
    {
        if (!allTasks.Contains(task))
        {
            allTasks.Add(task);
            taskUI.UpdateTaskList(allTasks);
        }
    }

    public void CompleteTask(GameTask task)
    {
        task.isCompleted = true;
        taskUI.UpdateTaskList(allTasks);
    }

    public void CompleteTaskForNPC(string npcName, string topic)
    {
        GameTask task = allTasks.Find(t => t.npcName == npcName && t.topic == topic);
        if (task != null && !task.isCompleted)
        {
            task.isCompleted = true;
            taskUI.UpdateTaskList(allTasks);
            Debug.Log($"Task '{task.taskName}' completed after talking to {npcName} about {topic}!");
        }
        else
        {
            Debug.LogWarning($"No matching task found for NPC: {npcName} with topic: {topic}");
        }
    }
}
