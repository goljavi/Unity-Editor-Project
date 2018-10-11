using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class QuestionNode : BaseEditorNode {
	new public Node node;
    public DialogueEditor dialogueEditorReference;


	public QuestionNode() {
		windowRect = new Rect(0, 0, 300, 100);
		nodeName = "Chat";
		node = new Node("");
	}

	public override void DrawWindow() {
		base.DrawWindow();
		GUIStyle style = GUI.skin.box;
		style.alignment = TextAnchor.MiddleCenter;

		EditorGUILayout.BeginHorizontal();
		node.text = EditorGUILayout.TextField(node.text);

		if (GUILayout.Button("Crear!"))
		{
            dialogueEditorReference.AddQuestionNode(node.AddOption("", node));
		}
		EditorGUILayout.EndHorizontal();

		for (int i = 0; i < node.options.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("-"))
			{
                node.options.RemoveAt(i);
				break;
			}

			node.options[i].Text = EditorGUILayout.TextField(node.options[i].Text);
			EditorGUILayout.EndHorizontal();
		}



	}

	public override void DrawCurve() {

	}
}
