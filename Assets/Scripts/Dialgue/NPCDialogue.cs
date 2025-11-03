using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue/NPC")]
public class NPCDialogue : ScriptableObject
{
    public TaskData unlockTask;
    public string topic;
    public DialogueTopic currentTopic;

    public string npcName;
    [TextArea(2, 5)]
    public string greetingText;
    public string greetingEmotion;
    public DialogueTopic[] dialogueTopics;
}

