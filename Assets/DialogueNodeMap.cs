using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Map", menuName = "Dialogue Map")]
public class DialogueNodeMap : ScriptableObject {
	public uint idCount;
    public List<BaseEditorNode> nodes;
}
