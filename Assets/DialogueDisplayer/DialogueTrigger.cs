using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DialogueBehavior))]
public class DialogueTrigger : MonoBehaviour {

    public string NPC_Name;
    public Sprite face;
    private DialogueBehavior db;
    public bool WorldSpaceDialogue = false;
    DialogueDisplayer dialogueDisplayer;

    
    public void Start()
    {
        db = GetComponent<DialogueBehavior>();
        if (WorldSpaceDialogue) dialogueDisplayer = GetComponent<DialogueDisplayer>();
        else dialogueDisplayer = DialogueDisplayer.instance;
    }

    public void TriggerDialogue()
    {
        dialogueDisplayer.StartDialogue(NPC_Name, face, db);
    }

    public void TriggerEndDialogue()
    {
        dialogueDisplayer.EndDialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("triggerd");
        if (WorldSpaceDialogue) TriggerDialogue();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (WorldSpaceDialogue) TriggerEndDialogue();
    }


}
