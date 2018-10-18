using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Este script que se encarga de recibir el archivo de tipo "DialogueNodeMap" 
 * y hacer de interfaz entre el sistema de nodos (servicio) y el script del programador (cliente)
 */
public class DialogueBehavior : MonoBehaviour
{
    [SerializeField]
    private DialogueNodeMap dialog;

    private DialogueObject first;
    private List<DialogueObject> dialogueObjects;
    private Dictionary<int, DialogueObject> optionIdToNextDialogue;

    void Start()
    {
        dialogueObjects = new List<DialogueObject>();
        optionIdToNextDialogue = new Dictionary<int, DialogueObject>();
        int start = 0;
        foreach (var node in dialog.nodes)
        {
            if (node.windowTitle == "Start")
            {
                start = node.id;
            }
            else if (node.windowTitle == "Dialogue")
            {
                dialogueObjects.Add(GetDialogueObject(node));
            }
        }

        GenerateIdToNextDictionary();
        GetFirstDialogue(start);
    }

    public DialogueObject GetStartingDialogue()
    {
        return GetFirstDialogue(0);
    }

    void GenerateIdToNextDictionary()
    {
        foreach (var diagObj in dialogueObjects)
        {
            foreach (var option in diagObj.optionsLinks)
            {
                foreach (var diagObj2 in dialogueObjects)
                {
                    if (diagObj2.id == option.Value) optionIdToNextDialogue.Add(option.Key, diagObj2);
                }
            }
        }
    }

    DialogueObject GetFirstDialogue(int startId)
    {
        if (first != null) return first;

        foreach (var node in dialog.nodes)
        {
            foreach (var parentId in node.parentIds)
            {
                if (parentId == startId)
                {
                    foreach(var dialogue in dialogueObjects)
                    {
                        if (node.id == dialogue.id)
                        {
                            first = dialogue;
                            return first;
                        }
                    }
                }
            }
        }

        return null;
    }

    public DialogueObject GetNextDialogue(int optionId)
    {
        if (!optionIdToNextDialogue.ContainsKey(optionId)) return null;
        return optionIdToNextDialogue[optionId];
    }

    DialogueObject GetDialogueObject(DialogueMapSerializedObject dialogueNode)
    {
        var dialogueObj = new DialogueObject
        {
            id = dialogueNode.id,
            dialogue = dialogueNode.data,
            options = GetDialogueOptions(dialogueNode)
        };

        dialogueObj.optionsLinks = GetOptionsLinks(dialogueObj.options);
        return dialogueObj;
    }

    public Dictionary<int, int> GetOptionsLinks(Dictionary<int, string> options)
    {
        var optionsLinks = new Dictionary<int, int>();
        foreach (var option in options)
        {
            foreach (var node in dialog.nodes)
            {
                foreach (var parentId in node.parentIds)
                {
                    if (option.Key == parentId) optionsLinks.Add(option.Key, node.id);
                } 
            }
        }
        return optionsLinks;
    }

    Dictionary<int, string> GetDialogueOptions(DialogueMapSerializedObject dialogueNode)
    {
        var options = new Dictionary<int, string>();

        foreach (var node in dialog.nodes)
        {
            if (node.windowTitle == "Option")
            {
                foreach(var parentId in node.parentIds)
                {
                    if (parentId == dialogueNode.id) options.Add(node.id, node.data);
                }
            }
        }

        return options;
    }
}