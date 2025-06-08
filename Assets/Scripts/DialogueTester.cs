using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Start()
    {
        string[] testDialogue = {
            "Hello, welcome to the township.",
            "Finding a job here is difficult, but stay hopeful.",
            "Try talking to the community leaders to learn more."
        };

        dialogueManager.StartDialogue(testDialogue);
    }
}
