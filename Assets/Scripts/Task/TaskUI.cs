using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public GameObject taskItemPrefab;  // Assign TaskItem prefab here
    public Transform taskListParent;   // Assign TaskListContainer here

    private List<GameObject> activeTaskItems = new List<GameObject>();

    public void UpdateTaskList(List<GameTask> tasks)
    {
        // Clear old task items
        foreach (var item in activeTaskItems)
            Destroy(item);
        activeTaskItems.Clear();

        // Create new task items and stack vertically
        foreach (var task in tasks)
        {
            GameObject newItem = Instantiate(taskItemPrefab, taskListParent);
            Text textComponent = newItem.GetComponent<Text>();
            string status = task.isCompleted ? "✔️ " : "❌ ";
            textComponent.text = status + task.taskName;

            activeTaskItems.Add(newItem);
        }
    }
}


