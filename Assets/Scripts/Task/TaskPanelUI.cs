using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskPanelUI : MonoBehaviour
{
    public GameObject taskItemPrefab;       // Prefab for each task
    public Transform taskListContainer;     // Container for task prefabs

    private void OnEnable()
    {
        RefreshTaskList();
    }

    public void RefreshTaskList()
    {
        // Clear old task items first to avoid duplicates
        foreach (Transform child in taskListContainer)
        {
            Destroy(child.gameObject);
        }

        // Loop through active tasks and create UI items
        foreach (var task in TaskManager.Instance.activeTasks)
        {
            GameObject go = Instantiate(taskItemPrefab, taskListContainer);
            go.transform.localScale = Vector3.one; // Fix scale if prefab imported weirdly

            TMP_Text nameText = go.transform.Find("TaskNameText")?.GetComponent<TMP_Text>();
            TMP_Text descText = go.transform.Find("TaskDescriptionText")?.GetComponent<TMP_Text>();
            Button completeBtn = go.transform.Find("CompleteButton")?.GetComponent<Button>();

            if (nameText == null || descText == null || completeBtn == null)
            {
                Debug.LogError("TaskItemPrefab child objects missing or misnamed!");
                continue;
            }

            nameText.text = task.taskName;
            descText.text = task.description;

            // Clear old listeners to avoid stacking multiple calls
            completeBtn.onClick.RemoveAllListeners();
            completeBtn.onClick.AddListener(() =>
            {
                TaskManager.Instance.CompleteTask(task);
                RefreshTaskList();
            });
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
