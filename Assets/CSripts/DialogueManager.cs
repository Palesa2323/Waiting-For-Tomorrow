using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public Button[] optionButtons; // 3 option buttons
    public Button nextButton;

    private DialogueTopic currentTopic;
    private int lineIndex = 0;

    void Start()
    {
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextLine);
    }

    public void StartDialogue(NPCDialogue npc)
    {
        dialoguePanel.SetActive(true);
        speakerNameText.text = npc.npcName;
        dialogueText.text = npc.greetingText;

        // Activate and setup choice buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = npc.dialogueTopics[i].playerChoiceText;

            int index = i; // capture index for closure
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => StartTopic(npc.dialogueTopics[index]));
        }

        nextButton.gameObject.SetActive(false); // hide "Next" until convo starts
    }

    void StartTopic(DialogueTopic topic)
    {
        currentTopic = topic;
        lineIndex = 0;

        // Hide option buttons
        foreach (Button btn in optionButtons)
            btn.gameObject.SetActive(false);

        nextButton.gameObject.SetActive(true); // show Next button
        ShowNextLine(); // start convo
    }

    public void ShowNextLine()
    {
        if (lineIndex >= currentTopic.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentTopic.lines[lineIndex];
        speakerNameText.text = line.speaker;
        dialogueText.text = line.text;

        lineIndex++;
    }

    public void EndDialogue()
    {
        currentTopic = null;
        lineIndex = 0;
        dialoguePanel.SetActive(false);
    }
}

