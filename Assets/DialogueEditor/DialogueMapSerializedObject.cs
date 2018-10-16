using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMapSerializedObject {
    public int id;
    public List<int> parentIds = new List<int>();
    public Rect windowRect;
    public string windowTitle;
}
