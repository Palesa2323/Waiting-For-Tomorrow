using UnityEngine;
using TMPro;
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

    void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(NextLine);

        completeTaskButton.gameObject.SetActive(false);
        completeTaskButton.onClick.AddListener(OnCompleteTaskClicked);
    }

    public void StartDialogue(NPCDialogue npc)
    {
        currentDialogue = npc;
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
            }
            else optionButtons[i].gameObject.SetActive(false);
        }

        continueButton.gameObject.SetActive(false);
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
        if (currentLineIndex < currentTopic.lines.Length) { ShowLine(); return; }

        // Unlock Task
        if (currentTopic.unlockTask != null)
        {
            GameTask task = currentTopic.unlockTask;
            task.sourceNPC = currentDialogue.npcName;
            task.topicId = currentTopic.id;
            TaskManager.Instance.UnlockTask(task);
        }
        if (taskPanel != null)
        {
            taskPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f; // pause the game while showing tasks
        }


        continueButton.gameObject.SetActive(false);
        completeTaskButton.gameObject.SetActive(true);
    }

    void OnCompleteTaskClicked()
    {
        completeTaskButton.gameObject.SetActive(false);
        if (taskPanel != null)
        {
            taskPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
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

        // Show task panel if needed
        if (taskPanel != null)
        {
            taskPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // Pause the game
        }

        currentDialogue = null;
    }

}
