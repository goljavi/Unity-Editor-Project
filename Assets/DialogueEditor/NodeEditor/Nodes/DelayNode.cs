﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DelayNode : BaseNode {
	public float num;
    

	public override string GetNodeType { get { return "Delay"; } }

	public override void DrawNode() {
        EditorGUILayout.LabelField("Delay in seconds:");
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

	public override bool CanTransitionTo(BaseNode node) {
		List<string> types = new List<string> { "Dialogue", "End", "Comparison", "Function" };

		return types.Contains(node.GetNodeType);
	}
}
