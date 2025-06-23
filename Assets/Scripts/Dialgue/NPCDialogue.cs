using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue/NPC")]
public class NPCDialogue : ScriptableObject
{
    public Task unlockTask;

    public string npcName;
    [TextArea(2, 5)]
    public string greetingText;
    public string greetingEmotion;
    public DialogueTopic[] dialogueTopics;
}

