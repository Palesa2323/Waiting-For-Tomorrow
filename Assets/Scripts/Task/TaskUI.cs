using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskUI : MonoBehaviour
{
    private TMP_Text taskText;

    void Awake()
    {
        // Finds the object named "TaskListText" in the scene and grabs its TMP_Text component
        GameObject textObj = GameObject.Find("TaskListText");
        if (textObj != null)
        {
            taskText = textObj.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("❌ TaskListText not found! Make sure it's named correctly.");
        }
    }

    public void UpdateTaskList(List<Task> tasks)
    {
        if (taskText == null) return;

        string output = "<b>📝 Tasks</b>\n";

        foreach (Task task in tasks)
        {
            output += task.isCompleted ? $"☑ {task.description}\n" : $"☐ {task.description}\n";
        }

        taskText.text = output;
    }
}

