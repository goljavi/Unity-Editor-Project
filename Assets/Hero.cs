using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    private DialogueBehavior _db;
    public Text content;
    public Text inputField;
    private DialogueObject current;
    private Dictionary<int, int> currentOptionsIds;

    public int oro = 100;

    void Start () {
        currentOptionsIds = new Dictionary<int, int>();
        _db = GetComponent<DialogueBehavior>();
        PrintDialogue(_db.GetStartingDialogue());
    }

    void PrintDialogue(DialogueObject dialogue)
    {
        if (dialogue == null)
        {
            WriteContent("Mago", "Adios!");
            return;
        }

        currentOptionsIds.Clear();

        current = dialogue;

        var dialogueString = current.dialogue + " \n";

        var contador = 0;
        foreach (var option in current.options)
        {
            contador++;
            dialogueString += "(" + contador + "): " + option.Value + " \n";
            currentOptionsIds.Add(contador, option.Key);
        }

        WriteContent("Mago", dialogueString);
    }
	
	void Update () {
        current.parameters.Setfloat("oro player", oro);
	}

    public void GetInputField()
    {
        int selectedValue = 0;
        
        if (!int.TryParse(inputField.text, out selectedValue) || !currentOptionsIds.ContainsKey(selectedValue))
        {
            WriteContent("Mago", "Que? No te entendí");
            return;
        }

        Debug.Log(currentOptionsIds[selectedValue]);
        PrintDialogue(_db.GetNextDialogue(currentOptionsIds[selectedValue]));
        inputField.text = "";
    }

    public void WriteContent(string agent, string text)
    {
        content.text += ">> " + agent + ": " + text + " \n\n";
    }

    public void Clear()
    {
        content.text = "";
    }
}
