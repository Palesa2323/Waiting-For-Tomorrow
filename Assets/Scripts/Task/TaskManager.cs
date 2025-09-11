using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    private List<GameTask> activeTasks = new List<GameTask>();
    public IReadOnlyList<GameTask> ActiveTasks => activeTasks;

    public event Action OnTaskListUpdated;
    public event Action<GameTask> OnTaskCompleted;
    public event Action<GameTask> OnTaskUnlocked;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // Optionally persist across scenes:
        // DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Adds a task to the active list. Safe to call multiple times - duplicates prevented.
    /// </summary>
    public void UnlockTask(GameTask task)
    {
        if (task == null)
        {
            Debug.LogWarning("TaskManager.UnlockTask called with null GameTask.");
            return;
        }

        if (activeTasks.Contains(task))
        {
            Debug.Log($"TaskManager: task already active: {task.taskName}");
            return;
        }

        activeTasks.Add(task);
        OnTaskListUpdated?.Invoke();
        OnTaskUnlocked?.Invoke(task);
        Debug.Log($"TaskManager: unlocked task '{task.taskName}' (source: {task.sourceNPC} topic: {task.topicId})");
    }

    /// <summary>
    /// Legacy AddTask alias (keeps compatibility).
    /// </summary>
    public void AddTask(GameTask task) => UnlockTask(task);

    /// <summary>
    /// Completes a specific TaskData and applies rewards.
    /// </summary>
    public void CompleteTask(GameTask task)
    {
        if (task == null) return;
        if (!activeTasks.Contains(task))
        {
            Debug.LogWarning($"TaskManager.CompleteTask: task not active: {task.taskName}");
            return;
        }

        // Apply rewards via ResourceManager (ensure it exists)
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.ChangeMoney(task.moneyReward);
            ResourceManager.Instance.ChangeFood(task.foodReward);
            ResourceManager.Instance.ChangeStress(task.stressChange);
        }
        else
        {
            Debug.LogWarning("TaskManager.CompleteTask: ResourceManager instance not found. Rewards not applied.");
        }

        activeTasks.Remove(task);
        OnTaskListUpdated?.Invoke();
        OnTaskCompleted?.Invoke(task);
        Debug.Log($"TaskManager: completed task '{task.taskName}' and applied rewards.");
    }

    /// <summary>
    /// Finds the active task that was unlocked by this NPC topic and completes it.
    /// Returns true if a task was found & completed.
    /// </summary>
    public bool CompleteTaskForNPC(string npcName, string topicId)
    {
        if (string.IsNullOrEmpty(npcName) || string.IsNullOrEmpty(topicId))
        {
            Debug.LogWarning("CompleteTaskForNPC called with empty npcName or topicId.");
            return false;
        }

        // Find first matching active task by metadata
        GameTask found = activeTasks.Find(t => t != null &&
                                               t.sourceNPC == npcName &&
                                               t.topicId == topicId);

        if (found != null)
        {
            CompleteTask(found);
            return true;
        }

        Debug.LogWarning($"TaskManager: No active task found for NPC '{npcName}' with topicId '{topicId}'.");
        return false;
    }

    /// <summary>
    /// Utility: remove task without applying reward (if needed)
    /// </summary>
    public void RemoveTask(GameTask task)
    {
        if (task == null) return;
        if (activeTasks.Remove(task)) OnTaskListUpdated?.Invoke();
    }

    /// <summary>
    /// Utility: check if a particular task is active
    /// </summary>
    public bool IsTaskActive(GameTask task) => task != null && activeTasks.Contains(task);
}
