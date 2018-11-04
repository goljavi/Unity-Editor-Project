using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FunctionNode : BaseNode {
	public float num;
	public override string GetNodeType { get { return "Function"; } }

	public override void DrawNode() {
        EditorGUILayout.LabelField("ID");
        var numVal = EditorGUILayout.FloatField(num);
		if (numVal != num)
		{
            num = numVal;
			reference.NotifyChangesWereMade();
		}
	}

	public override Color GetBackgroundColor() {

        defaultColor = Color.red;
        return color;
    }


    public override string GetNodeData() {
		return num.ToString();
	}

	public override void SetNodeData(string data) {
		num = float.Parse(data);
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
