using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DialogueBehavior))]
public class DialogueTrigger : MonoBehaviour {

    public string NPC_Name;
    public Sprite face;
    public DialogueBehavior db;

    public void Start()
    {
        db = GetComponent<DialogueBehavior>();
    }

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(NPC_Name, face, db);
    }

}
