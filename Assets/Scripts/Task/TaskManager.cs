using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    public List<GameTask> activeTasks = new List<GameTask>();

    public event Action OnTaskListUpdated;
    public event Action<GameTask> OnTaskCompleted;
    public event Action<GameTask> OnTaskUnlocked;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void UnlockTask(GameTask task)
    {
        if (task == null) return;
        if (!activeTasks.Contains(task))
        {
            activeTasks.Add(task);
            Debug.Log("✅ Task unlocked: " + task.taskName);
            OnTaskUnlocked?.Invoke(task);
            OnTaskListUpdated?.Invoke();
        }
    }

    public void CompleteTask(GameTask task)
    {
        if (task == null || !activeTasks.Contains(task)) return;

        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.ChangeMoney(task.moneyReward);
            ResourceManager.Instance.ChangeFood(task.foodReward);
            ResourceManager.Instance.ChangeStress(task.stressChange);
        }

        activeTasks.Remove(task);
        OnTaskListUpdated?.Invoke();
        OnTaskCompleted?.Invoke(task);
        Debug.Log($"✅ Task completed: {task.taskName}");

        // Load mini-game scene if assigned
        if (!string.IsNullOrEmpty(task.miniGameScene))
            UnityEngine.SceneManagement.SceneManager.LoadScene(task.miniGameScene);
    }
   


    public bool CompleteTaskForNPC(string npcName, string topicId)
    {
        GameTask found = activeTasks.Find(t => t.sourceNPC == npcName && t.topicId == topicId);
        if (found != null) { CompleteTask(found); return true; }
        Debug.LogWarning($"No active task found for NPC '{npcName}' with topic '{topicId}'");
        return false;
    }

    public void RemoveTask(GameTask task)
    {
        if (task == null) return;
        if (activeTasks.Remove(task)) OnTaskListUpdated?.Invoke();
    }

    public bool IsTaskActive(GameTask task) => task != null && activeTasks.Contains(task);
}
