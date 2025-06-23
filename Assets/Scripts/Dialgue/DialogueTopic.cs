using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Topic", menuName = "Dialogue/Topic")]
public class DialogueTopic : ScriptableObject
{
    public Task unlockTask; // 👈 This is the missing line!


    public string topicId;
    public string playerChoiceText;
    public DialogueLine[] lines;
}

