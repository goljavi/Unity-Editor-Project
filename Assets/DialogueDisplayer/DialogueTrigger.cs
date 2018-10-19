using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueBehavior))]
public class DialogueTrigger : MonoBehaviour {

    public string speakerName;
    public DialogueBehavior db;

    public void Start()
    {
        db = GetComponent<DialogueBehavior>();
    }

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(speakerName, db);
    }

}
