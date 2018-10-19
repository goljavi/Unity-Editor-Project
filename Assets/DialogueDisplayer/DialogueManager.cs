using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;

    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private DialogueBehavior db;
    private DialogueObject current;
    private Dictionary<int, int> currentOptionsIds;

    void Start () {

        if (instance == null) instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //        sentences = new Queue<string>();
        currentOptionsIds = new Dictionary<int, int>();


    }

    public void StartDialogue(string name, DialogueBehavior db)
    {
        animator.SetBool("isOpen", true);

        nameText.text = name;
        this.db = db;

        PrintDialogue(db.GetStartingDialogue());
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
            dialogueString += "(" + contador + "): " + option.Value + " \n";
            currentOptionsIds.Add(contador, option.Key);
        }

        DisplaySentence(dialogueString);
    }
}
