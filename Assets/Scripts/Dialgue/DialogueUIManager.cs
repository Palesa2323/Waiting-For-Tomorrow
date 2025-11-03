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

        // Assign task linked to this topic
        if (topic.unlockTask != null)
        {
            GameManager.Instance.currentTask = topic.unlockTask;
        }

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
        if (currentTopic == null)
        {
            Debug.LogWarning("NextLine called but currentTopic is null!");
            return;
        }

        if (currentTopic.lines == null || currentTopic.lines.Length == 0)
        {
            Debug.LogWarning("NextLine called but currentTopic.lines is null or empty!");
            return;
        }

        currentLineIndex++;
        if (currentLineIndex < currentTopic.lines.Length)
        {
            ShowLine();
            return;
        }

        // Unlock Task if there is one
        if (currentTopic.unlockTask != null)
        {
            if (TaskManagement.Instance != null)
            {
                TaskManagement.Instance.UnlockTask(currentTopic.unlockTask);
            }
            else
            {
                Debug.LogWarning("TaskManagement.Instance is null! Make sure TaskManagement exists in scene.");
            }
        }

        continueButton.gameObject.SetActive(false);
        completeTaskButton.gameObject.SetActive(true);
    }

    void OnCompleteTaskClicked()
    {
        if (GameManager.Instance.currentTask != null)
        {
            SceneManager.LoadScene("TaskScene");
        }
        else
        {
            Debug.LogWarning("No task assigned!");
        }
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
