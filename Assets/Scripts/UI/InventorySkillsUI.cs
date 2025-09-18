using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySkillsUI : MonoBehaviour
{
    public GameObject menuPanel;
    public Button inventoryTabButton;
    public Button skillsTabButton;
    public Transform listContent;
    public GameObject listItemPrefab;
    public TextMeshProUGUI descriptionText;

    public List<Item> playerItems;
    public List<Skill> playerSkills;

    void Start()
    {
        menuPanel.SetActive(false); // Hide by default
        inventoryTabButton.onClick.AddListener(ShowInventory);
        skillsTabButton.onClick.AddListener(ShowSkills);
    }

    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    void ShowInventory()
    {
        ClearList();
        foreach (var item in playerItems)
        {
            var entry = Instantiate(listItemPrefab, listContent);
            entry.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            entry.GetComponent<Button>().onClick.AddListener(() => ShowDescription(item.description));
        }
    }

    void ShowSkills()
    {
        ClearList();
        foreach (var skill in playerSkills)
        {
            if (skill.isUnlocked)
            {
                var entry = Instantiate(listItemPrefab, listContent);
                entry.GetComponentInChildren<TextMeshProUGUI>().text = skill.skillName;
                entry.GetComponent<Button>().onClick.AddListener(() => ShowDescription(skill.description));
            }
        }
    }

    void ShowDescription(string desc)
    {
        descriptionText.text = desc;
    }

    void ClearList()
    {
        foreach (Transform child in listContent)
        {
            Destroy(child.gameObject);
        }
    }
}
