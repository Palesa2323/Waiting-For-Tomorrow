using UnityEngine;

public class TaskPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject taskItemPrefab;
    [SerializeField] private Transform content;

    void OnEnable()
    {
        TaskManager.Instance.OnTaskListUpdated += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        TaskManager.Instance.OnTaskListUpdated -= Refresh;
    }

    void Refresh()
    {
        // Clear old items
        foreach (Transform child in content)
            Destroy(child.gameObject);

        // Rebuild list
        foreach (var task in TaskManager.Instance.ActiveTasks)
        {
            var item = Instantiate(taskItemPrefab, content);
            item.GetComponent<TaskUI>().Setup(task);
        }
    }
}
