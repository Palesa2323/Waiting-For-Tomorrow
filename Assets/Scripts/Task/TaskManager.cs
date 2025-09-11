using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    public List<GameTask> activeTasks = new List<GameTask>();

    public List<GameTask> ActiveTasks => activeTasks; // public getter

    public event Action OnTaskListUpdated;
    public event Action<GameTask> OnTaskCompleted;
    public event Action<GameTask> OnTaskUnlocked;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // optional
    }

    /// <summary>
    /// Unlocks a task, adds it to activeTasks.
    /// </summary>
    public void UnlockTask(GameTask task)
    {
        if (task == null) return;

        if (!activeTasks.Contains(task))
        {
            activeTasks.Add(task);
            Debug.Log("✅ Task unlocked: " + task.taskName);
            OnTaskUnlocked?.Invoke(task);
            OnTaskListUpdated?.Invoke(); // notify UI to refresh
        }
    }

    public void AddTask(GameTask task) => UnlockTask(task);

    /// <summary>
    /// Completes a task and applies rewards.
    /// </summary>
    public void CompleteTask(GameTask task)
    {
        if (task == null) return;

        if (!activeTasks.Contains(task))
        {
            Debug.LogWarning($"TaskManager.CompleteTask: task not active: {task.taskName}");
            return;
        }

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
        Debug.Log($"✅ Task completed: {task.taskName}");
    }

    /// <summary>
    /// Completes a task for a specific NPC/topic
    /// </summary>
    public bool CompleteTaskForNPC(string npcName, string topicId)
    {
        if (string.IsNullOrEmpty(npcName) || string.IsNullOrEmpty(topicId))
        {
            Debug.LogWarning("CompleteTaskForNPC called with empty npcName or topicId.");
            return false;
        }

        GameTask found = activeTasks.Find(t => t != null &&
                                               t.sourceNPC == npcName &&
                                               t.topicId == topicId);

        if (found != null)
        {
            CompleteTask(found);
            return true;
        }

        Debug.LogWarning($"No active task found for NPC '{npcName}' with topicId '{topicId}'");
        return false;
    }

    public void RemoveTask(GameTask task)
    {
        if (task == null) return;
        if (activeTasks.Remove(task)) OnTaskListUpdated?.Invoke();
    }

    public bool IsTaskActive(GameTask task) => task != null && activeTasks.Contains(task);
}
