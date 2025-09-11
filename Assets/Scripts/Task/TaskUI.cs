using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TMP_Text taskNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button completeButton;

    private GameTask task;

    public void Setup(GameTask taskData)
    {
        task = taskData;
        taskNameText.text = task.taskName;
        descriptionText.text = task.description;

        completeButton.onClick.RemoveAllListeners();
        completeButton.onClick.AddListener(() =>
        {
            TaskManager.Instance.CompleteTask(task);
            Destroy(gameObject); // remove from UI
        });
    }
}


