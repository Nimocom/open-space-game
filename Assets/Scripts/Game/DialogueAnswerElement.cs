using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueAnswerElement : MonoBehaviour, IPointerClickHandler
{
    public string answerText;
    public Text answerUI;
    public int toNodeIndex;
    public int controller;
    public bool isDialogueEnd;
    public bool isSuccess;
    public bool isFail;
    public bool isQuestStarter;

    void Start()
    {
        answerUI.text = answerText;
    }
        

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDialogueEnd)
        {
            DialogueManager.inst.FinishDialogue();
            return;
        }


        DialogueManager.inst.SetNode(toNodeIndex);
    }
}
