using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    public int currentDay = 1;
    public float dayLength = 1000f; // 1 in-game day = 60 seconds
    private float dayTimer;

    public TextMeshProUGUI dayText; // Assign in inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        dayTimer = dayLength;
        UpdateDayUI();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return; // pause timer if needed

        dayTimer -= Time.deltaTime;

        if (dayTimer <= 0f)
            NextDay();
    }

    private void NextDay()
    {
        currentDay++;
        dayTimer = dayLength;
        UpdateDayUI();
        Debug.Log("➡️ New Day: " + currentDay);

        if (DailyGoalManager.Instance != null)
            DailyGoalManager.Instance.NextDay();

    }

    private void UpdateDayUI()
    {
        if (dayText != null)
            dayText.text = "Day " + currentDay;
    }


    // Optional: manually advance day
    public void AdvanceDay() => dayTimer = 0f;
}
