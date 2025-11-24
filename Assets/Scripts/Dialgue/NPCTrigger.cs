using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public NPCDialogue npcDialogue;

    private DialogueUIManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueUIManager>();
        if (dialogueManager == null)
            Debug.LogError("DialogueUIManager NOT found in scene 😤");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dialogueManager != null)
        {
            // Start the dialogue; pass a callback if needed
            dialogueManager.StartDialogue(npcDialogue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && dialogueManager != null)
        {
            dialogueManager.EndDialogue();
        }
    }

    // Hook this to the Next button in the UI
    public void NextDialogueLine()
    {
        if (dialogueManager != null)
        {
            dialogueManager.NextLine(); // Calls the existing NextLine method
        }
    }
}
