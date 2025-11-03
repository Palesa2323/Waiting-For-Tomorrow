using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerText, dialogueText;
    public Button continueButton, completeTaskButton;
    public Button[] optionButtons;
    public GameObject taskPanel;

    private NPCDialogue currentDialogue;
    private DialogueTopic currentTopic;
    private int currentLineIndex;
    public TaskData unlockTask; // always use TaskData



    public delegate void DialogueCompletionCallback();
    private DialogueCompletionCallback onDialogueEnd;

    private void Start()
    {
        // Assign button callbacks once
        if (continueButton != null) continueButton.onClick.AddListener(NextLine);
        if (completeTaskButton != null) completeTaskButton.onClick.AddListener(OnCompleteTaskClicked);

        // Hide panels at start
        dialoguePanel.SetActive(false);
        if (taskPanel != null) taskPanel.SetActive(false);
    }

    public void StartDialogue(NPCDialogue npc, DialogueCompletionCallback callback = null)
    {
        currentDialogue = npc;
        onDialogueEnd = callback;

        // Show dialogue panel
        dialoguePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; // pause game

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
        completeTaskButton.gameObject.SetActive(false);
    }

    void StartTopic(DialogueTopic topic)
    {
        currentTopic = topic;
        currentLineIndex = 0;

        foreach (Button btn in optionButtons) btn.gameObject.SetActive(false);
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
            return;
        }

        // Unlock Task if there is one
        if (currentTopic.unlockTask != null)
        {
            TaskData task = currentTopic.unlockTask;

             TaskManagement.Instance.UnlockTask(currentTopic.unlockTask); 
        }

        continueButton.gameObject.SetActive(false);
        completeTaskButton.gameObject.SetActive(true);
    }

    void OnCompleteTaskClicked()
    {
        completeTaskButton.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);

        // Restore cursor & time before switching scene
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;

        // Pass info about which task to complete
        GameManager.Instance.currentTask = currentTopic.unlockTask; // store TaskData somewhere global

        SceneManager.LoadScene("TaskScene"); // load the task scene
    }

    public void CloseTaskPanel()
    {
        if (taskPanel != null)
        {
            taskPanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentTopic = null;
        currentLineIndex = 0;

        if (onDialogueEnd != null)
        {
            onDialogueEnd.Invoke();
            onDialogueEnd = null;
        }

        currentDialogue = null;

        // Ensure cursor & time are restored
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}
