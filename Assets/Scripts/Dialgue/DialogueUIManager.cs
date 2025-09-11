using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Button continueButton;
    public Button completeTaskButton; 

    public Button[] optionButtons;
    public GameObject taskPanel;  

    private NPCDialogue currentDialogue;
    private DialogueTopic currentTopic;
    private int currentLineIndex = 0;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(NextLine);

        completeTaskButton.gameObject.SetActive(false);  // Hide initially
        completeTaskButton.onClick.AddListener(OnCompleteTaskClicked);
    }

    public void StartDialogue(NPCDialogue npc)
    {
        currentDialogue = npc; // ✅ FIXED! Don't use uninitialized `dialogue` field
        Debug.Log("Starting dialogue with NPC: " + npc.npcName);

        dialoguePanel.SetActive(true);
        speakerText.text = npc.npcName;
        dialogueText.text = npc.greetingText;

        // Display available topics
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < npc.dialogueTopics.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = npc.dialogueTopics[i].playerChoiceText;

                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => StartTopic(npc.dialogueTopics[index]));
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
            // Topic finished
            Debug.Log("Reached end of topic.");

            // Unlock the task if there is one
            if (currentTopic.unlockTask != null)
            {
                // Set the metadata so TaskManager can find it
                GameTask task = currentTopic.unlockTask;
                task.sourceNPC = currentDialogue.npcName;
                task.topicId = currentTopic.id;

                TaskManager.Instance.UnlockTask(task);
                Debug.Log("✅ Task unlocked: " + task.taskName);
            }

            // Hide continue button, show Complete Task button
            continueButton.gameObject.SetActive(false);
            completeTaskButton.gameObject.SetActive(true);
        }
    }


    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentTopic = null;
        currentLineIndex = 0;

        // 🧾 Show task panel
        if (taskPanel != null)
        {
            taskPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // Pause game
        }

        currentDialogue = null;
    }

    public void CloseTaskPanel()
    {
        if (taskPanel != null)
        {
            taskPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    void OnCompleteTaskClicked()
    {
        completeTaskButton.gameObject.SetActive(false);

        if (taskPanel != null)
        {
            taskPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f; // pause game
        }

        EndDialogue();
    }

}
