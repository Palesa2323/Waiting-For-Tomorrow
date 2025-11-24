using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{

    public GameObject dialoguePanel;
    public TMP_Text speakerText, dialogueText;
    public Button continueButton;
    public GameObject taskPanel;  
    public Button completionButton; 

    public Button[] optionButtons;

    private NPCDialogue currentDialogue;
    private DialogueTopic currentTopic;
    private int currentLineIndex;

    public delegate void DialogueCompletionCallback();
    private DialogueCompletionCallback onDialogueEnd;

    private void Start()
    {
 
        if (continueButton != null) continueButton.onClick.AddListener(NextLine);

        // No need to assign OnCompleteTaskClicked listener if the button is removed/unused.
        // if (completeTaskButton != null) completeTaskButton.onClick.AddListener(OnCompleteTaskClicked); 

        // Hide panels at start
        dialoguePanel.SetActive(false);
        if (taskPanel != null) taskPanel.SetActive(false);
    }

    // A flag to check if dialogue is currently running (useful for NPCTrigger)
    public bool IsDialogueActive() => dialoguePanel.activeInHierarchy;
    public void StartDialogue(NPCDialogue npc, DialogueCompletionCallback callback = null)
    {
        currentDialogue = npc;
        onDialogueEnd = callback;

        // Show dialogue panel and pause the game
        dialoguePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        // Set greeting
        speakerText.text = npc.npcName;
        dialogueText.text = npc.greetingText;

        // Setup choice buttons
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
            else optionButtons[i].gameObject.SetActive(false);
        }

        continueButton.gameObject.SetActive(false);
        // completeTaskButton.gameObject.SetActive(false); // Removed button reference
    }

    void StartTopic(DialogueTopic topic)
    {
        currentTopic = topic;
        currentLineIndex = 0;

        foreach (Button btn in optionButtons) btn.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);

        // This line was wrong: GameManager.Instance.currentTask = topic.unlockTask;
        // The task is passed to TaskManagement *only* when the dialogue finishes (in NextLine).
        // No action is needed here, as the task is stored in the topic itself.

        ShowLine();
    }

    void ShowLine()
    {
        if (currentTopic != null && currentLineIndex < currentTopic.lines.Length)
        {
            DialogueLine line = currentTopic.lines[currentLineIndex];
            speakerText.text = line.speaker;
            dialogueText.text = line.text;
        }
    }


    public void NextLine()
    {
        // --- Safety Checks ---
        if (currentTopic == null || currentTopic.lines == null || currentTopic.lines.Length == 0)
        {
            Debug.LogWarning("NextLine called but currentTopic is invalid!");
            EndDialogue(); // Force close if something is wrong
            return;
        }

        currentLineIndex++;
        if (currentLineIndex < currentTopic.lines.Length)
        {
            ShowLine();
            return; // Continue dialogue lines
        }

        if (currentTopic.unlockTask != null)
        {
            if (TaskManagement.Instance != null)
            {
                // TaskManagement handles the UI (pausing, showing the panel, setting Time.timeScale=0)
                TaskManagement.Instance.UnlockTask(currentTopic.unlockTask);
            }
            else
            {
                Debug.LogWarning("TaskManagement.Instance is null! Cannot show task panel. Calling EndDialogue.");
                EndDialogue();
            }
        }
        else
        {
            // No task to unlock, just end the conversation
            EndDialogue();
        }

        // We do not need to set the continueButton to false here, 
        // as EndDialogue() or TaskManagement.UnlockTask() will cover the state change.
    }
    public void ShowTaskPanel()
    {
        // Hide the completion BUTTON after pressing
        if (completionButton != null)
            completionButton.gameObject.SetActive(false);

        // Show the Task panel
        if (taskPanel != null)
            taskPanel.SetActive(true);

        // Keep UI mode (cursor free)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentTopic = null;
        currentLineIndex = 0;

        // 1. Invoke the callback provided by the NPCTrigger
        if (onDialogueEnd != null)
        {
            onDialogueEnd.Invoke();
            onDialogueEnd = null;
        }

        currentDialogue = null;

        // 2. Ensure cursor & time are restored ONLY if the Task Panel IS NOT active.
        // If the Task Panel is open, it should control time/cursor state.
        if (TaskManagement.Instance == null || !TaskManagement.Instance.taskPanel.activeInHierarchy)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }

    // This function is useful if the Task Panel wants the Dialogue Manager to handle restoration.
    public void RestorePlayerControl()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}