using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FunctionNode : BaseNode {
	public string text;
    private SerializedObject serializedObject;

    public override string GetNodeType { get { return "Function"; } }

	public override void DrawNode() {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("onSkillUse"), new GUIContent("On Skill Use"));
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
