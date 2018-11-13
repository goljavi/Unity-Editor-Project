using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : BaseNode {
	public string text;

    public override string GetNodeType { get { return "Dialogue"; } }

    public override void DrawNode()
    {
		EditorStyles.textArea.wordWrap = true;
		var textValue = EditorGUILayout.TextArea(text, EditorStyles.textArea, GUILayout.Height(80));
		if (textValue != text)
		{
			text = textValue;
			reference.NotifyChangesWereMade();
		}
	}

	public override Color GetBackgroundColor() {

        defaultColor = Color.green;
        return color;
    }


    public override string GetNodeData() {
		return text;
	}

	public override BaseNode SetNodeData(string data) {
		text = data;
		return this;
	}

	public override void DrawConnection() {
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
		List<string> types = new List<string> { "Option", "End" };

		return types.Contains(node.GetNodeType);
	}
}
