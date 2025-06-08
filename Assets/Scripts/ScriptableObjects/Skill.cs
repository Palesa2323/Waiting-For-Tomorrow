using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    [TextArea]
    public string description;
    public bool isUnlocked;
}

