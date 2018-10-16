using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Map", menuName = "Dialogue Map")]
public class DialogueNodeMap : ScriptableObject
{
    public List<DialogueMapSerializedObject> nodes = new List<DialogueMapSerializedObject>();
}