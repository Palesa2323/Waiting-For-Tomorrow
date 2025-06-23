using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Topic", menuName = "Dialogue/Topic")]
public class DialogueTopic : ScriptableObject
{
  
    public string topicId;
    public string playerChoiceText;
    public DialogueLine[] lines;
}

