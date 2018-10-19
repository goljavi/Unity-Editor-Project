using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : BaseNode {
	public string text;

	public override string GetNodeType { get { return "Dialogue"; } }

	public override void DrawNode() {
		text = EditorGUILayout.TextArea(text, GUILayout.Height(80));
	}

	public override string GetNodeData() {
		return text;
	}

	public override void SetNodeData(string data) {
		text = data;
	}

}
