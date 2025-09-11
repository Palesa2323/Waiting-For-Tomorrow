using UnityEngine;
using TMPro;          // for TextMeshPro
using UnityEngine.UI; // <-- this one is needed for Button, Image, Slider, etc.


public class TaskPanelUI : MonoBehaviour
{
    public GameObject taskItemPrefab; // prefab
    public Transform taskListContainer; // parent container

    void OnEnable()
    {
        RefreshTaskList();
    }

    public void RefreshTaskList()
    {
        // Clear existing items
        foreach (Transform child in taskListContainer)
            Destroy(child.gameObject);

        foreach (var task in TaskManager.Instance.activeTasks)
        {
            GameObject go = Instantiate(taskItemPrefab, taskListContainer);
            go.transform.localScale = Vector3.one;

            TMP_Text nameText = go.transform.Find("TaskNameText").GetComponent<TMP_Text>();
            TMP_Text descText = go.transform.Find("TaskDescriptionText").GetComponent<TMP_Text>();
            Button completeBtn = go.transform.Find("CompleteButton").GetComponent<Button>();

            if (nameText == null || descText == null || completeBtn == null)
            {
                Debug.LogError("TaskItemPrefab child objects missing or misnamed!");
                continue;
            }
            nameText.text = task.taskName;
            descText.text = task.description;

            completeBtn.onClick.RemoveAllListeners();
            completeBtn.onClick.AddListener(() =>
            {
                TaskManager.Instance.CompleteTask(task);
                RefreshTaskList(); // update UI
            });
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f; // unpause
    }
}
