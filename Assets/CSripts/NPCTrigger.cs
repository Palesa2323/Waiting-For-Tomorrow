using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public NPCDialogue npcDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<DialogueManager>().StartDialogue(npcDialogue);
        }
    }
}

