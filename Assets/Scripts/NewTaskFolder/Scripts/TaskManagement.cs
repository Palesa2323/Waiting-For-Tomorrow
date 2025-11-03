using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TaskManagement : MonoBehaviour
{
    public static TaskManagement Instance;
    void Awake() { Instance = this; }

    [Header("UI References")]
    public GameObject taskPanel;
    public TextMeshProUGUI taskNameText;
    public TextMeshProUGUI taskDescText;
    public Button acceptButton;
    public Button declineButton;

    private TaskData currentTask;

    private void Start()
    {
        if (acceptButton != null) acceptButton.onClick.AddListener(AcceptTask);
        if (declineButton != null) declineButton.onClick.AddListener(DeclineTask);

        if (taskPanel != null) taskPanel.SetActive(false);
    }

    public void ShowTaskPanel(TaskData task)
    {
        currentTask = task;

        if (taskPanel != null)
        {
            taskPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

            if (taskNameText != null) taskNameText.text = task.taskName;
            if (taskDescText != null) taskDescText.text = task.taskDescription;

            Debug.Log($"Task Panel opened for: {task.taskName}");
        }
        else Debug.LogError("Task Panel not assigned in TaskManagement!");
    }

    public void UnlockTask(TaskData task)
    {
        ShowTaskPanel(task);
    }

    private void AcceptTask()
    {
        taskPanel.SetActive(false);

        GameManager.Instance.money += currentTask.moneyReward;
        GameManager.Instance.happiness += currentTask.happinessReward;
        GameManager.Instance.stress += currentTask.stressChange;

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (currentTask.requiresMiniGame)
            SceneManager.LoadScene(currentTask.miniGameSceneName);
        else
            GameManager.Instance.NextDay();
    }

    private void DeclineTask()
    {
        taskPanel.SetActive(false);

        GameManager.Instance.stress += currentTask.declineStressIncrease;
        GameManager.Instance.happiness += currentTask.declineHappinessPenalty;

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.NextDay();
    }
}
