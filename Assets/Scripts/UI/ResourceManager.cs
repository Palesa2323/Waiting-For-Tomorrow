using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Resources")]
    public int Money = 0;
    public int Food = 0;
    public float Stress = 0f;
    public float Hunger = 0f;

    // Events for UI
    public event System.Action<int> OnMoneyChanged;
    public event System.Action<int> OnFoodChanged;
    public event System.Action<float> OnStressChanged;
    public event System.Action<float> OnHungerChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // optional, keeps it alive across scenes
    }

    // Methods to change resources
    public void ChangeMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
    }

    public void ChangeFood(int amount)
    {
        Food += amount;
        OnFoodChanged?.Invoke(Food);
    }

    public void ChangeStress(float amount)
    {
        Stress += amount;
        Stress = Mathf.Clamp(Stress, 0f, 100f);
        OnStressChanged?.Invoke(Stress);
    }

    public void ChangeHunger(float amount)
    {
        Hunger += amount;
        Hunger = Mathf.Clamp(Hunger, 0f, 100f);
        OnHungerChanged?.Invoke(Hunger);
    }
}
