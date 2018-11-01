using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OptionNode : BaseNode {
	public string text;
    

	public override string GetNodeType { get { return "Option"; } }

	public override void DrawNode() {
		EditorStyles.textArea.wordWrap = true;
		var textValue = EditorGUILayout.TextArea(text, EditorStyles.textArea, GUILayout.Height(80));
		if (textValue != text)
		{
			text = textValue;
			reference.NotifyChangesWereMade();
		}
	}

	public override Color GetBackgroundColor() {

        defaultColor = Color.red;
        return color;
    }


    public override string GetNodeData() {
		return text;
	}

	public override void SetNodeData(string data) {
		text = data;
	}

	public override void DrawConnection() {
		if (parents.Count > 0)
		{
			foreach (var parent in parents)
			{
				if (parent != null) DialogueEditor.DrawNodeConnection(parent.windowRect, windowRect, true, Color.black);
			}
		}
	}
}
