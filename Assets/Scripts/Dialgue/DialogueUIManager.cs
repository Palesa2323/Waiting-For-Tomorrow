using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Button continueButton;
    public Button[] optionButtons;
    public GameObject taskPanel;  // Assign your TaskPanel here in Inspector
    private NPCDialogue currentDialogue;  // Your existing dialogue data ref
   



    private DialogueTopic currentTopic;
    private int currentLineIndex = 0;
    private NPCDialogue dialogue;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("DialogueUIManager is ready.");
        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue(NPCDialogue npc)
    {
        currentDialogue = dialogue;
        Debug.Log("Starting dialogue with NPC: " + npc.npcName);

        if (npc.dialogueTopics.Length < optionButtons.Length)
        {
            Debug.LogWarning("Not enough topics to fill all buttons.");
        }

        dialoguePanel.SetActive(true);
        speakerText.text = npc.npcName;
        dialogueText.text = npc.greetingText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < npc.dialogueTopics.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = npc.dialogueTopics[i].playerChoiceText;

                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => StartTopic(npc.dialogueTopics[index]));

                Debug.Log("Option button " + i + " assigned to topic: " + npc.dialogueTopics[i].playerChoiceText);
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }

        continueButton.gameObject.SetActive(false);
    }

    void StartTopic(DialogueTopic topic)
    {
        currentTopic = topic;
        currentLineIndex = 0;

        Debug.Log("Started topic: " + topic.playerChoiceText + " with " + topic.lines.Length + " lines.");

        foreach (Button btn in optionButtons)
            btn.gameObject.SetActive(false);

        continueButton.gameObject.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLineIndex < currentTopic.lines.Length)
        {
            DialogueLine line = currentTopic.lines[currentLineIndex];
            speakerText.text = line.speaker;
            dialogueText.text = line.text;
            Debug.Log($"Showing line {currentLineIndex}: [{line.speaker}] {line.text}");
        }
        else
        {
            Debug.LogWarning("Tried to show a line out of bounds.");
            EndDialogue();
        }
    }

    void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < currentTopic.lines.Length)
        {
            ShowLine();
        }
        else
        {
            Debug.Log("Reached end of topic.");

            if (currentTopic.unlockTask != null)
            {
                TaskManager.Instance.UnlockTask(currentTopic.unlockTask);
                Debug.Log("✅ Task unlocked: " + currentTopic.unlockTask.taskName);
            }

            EndDialogue();
        }

    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("Dialogue panel closed.");
        currentTopic = null;
        currentLineIndex = 0;
        if (currentDialogue != null)
        {
            TaskManager.Instance.CompleteTaskForNPC(currentDialogue.npcName, currentDialogue.topic);
            currentDialogue = null;
        }

        if (taskPanel != null)
        {
            taskPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;  // Pause game while task panel is open
        }

    }

    public void CloseTaskPanel()
    {
        if (taskPanel != null)
        {
            taskPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f; // Resume game
        }
    }

}
