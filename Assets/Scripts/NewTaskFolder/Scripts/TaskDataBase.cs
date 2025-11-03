using System.Collections.Generic;
using UnityEngine;

public class TaskDatabase : MonoBehaviour
{
    public static TaskDatabase Instance;
    void Awake() { Instance = this; }

    public List<TaskData> allTasks = new List<TaskData>();
}
