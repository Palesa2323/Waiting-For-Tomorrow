using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public NPCDialogue testNPCDialogue; // drag an SO into this in the Inspector

    void Start()
    {
        dialogueManager.StartDialogue(testNPCDialogue);
    }
}
