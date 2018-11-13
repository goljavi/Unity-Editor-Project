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

	public override BaseNode SetNodeData(string data) {
		num = float.Parse(data);
		return this;
	}

    public override void DrawConnection()
    {
        if (parents.Count > 0)
        {
            foreach (var parent in parents)
            {
                if (parent == null) continue;

                var finalcolor = Color.white;
                if (parent.GetNodeType == "Comparison")
                {
                    var compnode = (ComparativeNode)parent;

                    if (System.Array.IndexOf(compnode.children, (BaseNode)this) == 0) finalcolor = Color.green;
                    else finalcolor = Color.red;
                }

                DialogueEditor.DrawNodeConnection(parent.windowRect, windowRect, true, finalcolor);
            }
        }
    }

    public override bool CanTransitionTo(BaseNode node) {
		List<string> types = new List<string> { "Dialogue", "End", "Comparison", "Delay" };

		return types.Contains(node.GetNodeType);
	}
}
