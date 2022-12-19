using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour 
{
    public static DialogueManager inst;

    public GameObject dialoguePanel;
    public DialogueInstance currentDialogue;
    public Text nodeText;
    public Transform answersBlock;
    public DialogueAnswerElement answerElement;
    public DialWrapper wrapper = new DialWrapper();
    AudioSource audioSource;

    void Awake()
    {
        inst = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void SetNode(int nodeIndex)
    {
        nodeText.text = currentDialogue.nodes[nodeIndex].nodeText;

        for (int i = 0; i < answersBlock.childCount; i++)
        {
            Destroy(answersBlock.GetChild(i).gameObject);
        }

        foreach (var answer in currentDialogue.nodes[nodeIndex].answers)
        {
            DialogueAnswerElement element;
            element = Instantiate(answerElement, answersBlock);
            element.answerText = answer.answerText;
            element.toNodeIndex = answer.answerNodeIndex;
            element.isDialogueEnd = answer.isDialogueEnd;
        }
    }

    public void StartDialogue(string dialogueID)
    {
        Time.timeScale = 0;
        currentDialogue = LocaManager.dialogues[dialogueID];
        SetNode(currentDialogue.nodes[0].nodeIndex);
        dialoguePanel.SetActive(true);
        audioSource.Play();
        UIManager.inst.SetCursorToArrow();
    }

    public void FinishDialogue()
    {
        Time.timeScale = 1f;
        dialoguePanel.SetActive(false);
        PlayerController.inst.isControlBlocked = false;
        UIManager.inst.SetCursorToAim();
    }
}
    

[System.Serializable]
public class Node
{
    public string nodeText;
    public int nodeIndex;
    public List<Answer> answers = new List<Answer>();
}

[System.Serializable]
public class Answer
{
    public string answerText;
    public int answerNodeIndex;
    public bool isDialogueEnd;
}

[System.Serializable]
public class DialogueInstance
{
    public Node[] nodes;
    public string dialogueID;
}

[System.Serializable]
public class DialWrapper
{
    public DialogueInstance[] dialogues;
}