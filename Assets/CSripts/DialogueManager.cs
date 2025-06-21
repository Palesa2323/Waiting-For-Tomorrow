using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public Button[] optionButtons;
    public Button nextButton;

    private DialogueTopic selectedTopic;
    private int lineIndex = 0;

    public void StartDialogue(NPCDialogue npc)
    {
        dialoguePanel.SetActive(true);
        speakerNameText.text = npc.npcName;
        dialogueText.text = npc.greetingText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = npc.dialogueTopics[i].playerChoiceText;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => StartTopic(npc.dialogueTopics[index]));
        }

        nextButton.gameObject.SetActive(false);
    }

    void StartTopic(DialogueTopic topic)
    {
        selectedTopic = topic;
        lineIndex = 0;
        foreach (var btn in optionButtons) btn.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (selectedTopic == null || lineIndex >= selectedTopic.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = selectedTopic.lines[lineIndex];
        speakerNameText.text = line.speaker;
        dialogueText.text = line.text;
        lineIndex++;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        selectedTopic = null;
        lineIndex = 0;
    }
}

