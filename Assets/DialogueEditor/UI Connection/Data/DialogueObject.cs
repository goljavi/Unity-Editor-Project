using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject {
    public int id;
    public string dialogue;
    public Dictionary<int, int> optionsLinks = new Dictionary<int, int>();
    public Dictionary<int, string> options = new Dictionary<int, string>();
}
