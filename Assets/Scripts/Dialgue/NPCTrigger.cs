using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public NPCDialogue npcDialogue;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered NPC trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger for: " + gameObject.name);

            DialogueUIManager dialogueManager = FindFirstObjectByType<DialogueUIManager>();
            if (dialogueManager != null)
            {
                Debug.Log("DialogueUIManager found! Starting dialogue...");
                dialogueManager.StartDialogue(npcDialogue);
            }
            else
            {
                Debug.LogError("DialogueUIManager NOT found in scene 😤");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger for: " + gameObject.name);

            DialogueUIManager dialogueManager = FindFirstObjectByType<DialogueUIManager>();
            if (dialogueManager != null)
            {
                dialogueManager.EndDialogue();
                Debug.Log("Dialogue ended.");
            }
        }
    }
}

