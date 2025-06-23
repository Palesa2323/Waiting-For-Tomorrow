using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskText;

    public void UpdateTaskList(List<Task> tasks)
    {
        string output = "";

        foreach (Task task in tasks)
        {
            output += task.isCompleted ? $"☑ {task.description}\n" : $"☐ {task.description}\n";
        }

        taskText.text = output;
    }
}
