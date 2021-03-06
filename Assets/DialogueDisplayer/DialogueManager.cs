﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;

    public Text nameText;
    public Image characterImage;
    public Text dialogueText;
    public Text[] optionsText;
    public int availableOptionSlots;

    public Animator animator;

    private DialogueBehavior db;
    private DialogueObject current;
    private Dictionary<int, int> currentOptionsIds;

    void Start () {

        if (instance == null) instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //new stuff below
        currentOptionsIds = new Dictionary<int, int>();

        availableOptionSlots = optionsText.Length;


    }

    public void StartDialogue(string name, Sprite face, DialogueBehavior db)
    {
        animator.SetBool("isOpen", true);
        SetFace(face);
        nameText.text = name;
        this.db = db;

        PrintDialogue(db.GetStartingDialogue());
    }

    public void SetFace(Sprite face)
    {
        if (face != null)
        {
            characterImage.sprite = face;
            characterImage.enabled = true;
        }
        else
        {
            characterImage.sprite = null;
            characterImage.enabled = false;
        }
    }

    public void DisplaySentence(string sentence)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        Debug.Log("Ending Conversation");
    }


    //Should refactor below
    void PrintDialogue(DialogueObject dialogue)
    {
        if (dialogue == null)
        {
            DisplaySentence("Adios!");
            return;
        }

        currentOptionsIds.Clear();

        current = dialogue;

        var dialogueString = current.dialogue + " \n";

        var contador = 0;
        foreach (var option in current.options)
        {
            contador++;
            DisplayOption("(" + contador + "): " + option.Value + " \n");
            currentOptionsIds.Add(contador, option.Key);
        }

        DisplaySentence(dialogueString);
    }

    void DisplayOption(string option)
    {
        if (availableOptionSlots < 1)
        {
            Debug.Log("Run out of space to display options");
            return;
        }
        availableOptionSlots -= 1;
        optionsText[availableOptionSlots].text = option;
    }

    public void ChooseOption(int selectedOption)
    {
        int selectedValue = selectedOption;

        if (!currentOptionsIds.ContainsKey(selectedOption))
        {
            Debug.Log("ERROR! Invalid option index!");
            return;
        }
        ClearOptions();
        PrintDialogue(db.GetNextDialogue(currentOptionsIds[selectedValue]));
    }

    void ClearOptions()
    {
        foreach(Text text in optionsText)
        {
            text.text = "";
        }

        availableOptionSlots = optionsText.Length;
    }
}
