using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SA;


public class QuestionNode : BaseEditorNode {
	private Vector2 textAreaSize = new Vector2(0, 0);
	new private Node node;

	public QuestionNode() {
		windowRect = new Rect(0, 0, 300, 100);
		nodeName = "Chat";
		node = new Node("");
	}

	public override void DrawWindow() {
		base.DrawWindow();
		GUIStyle style = GUI.skin.box;
		style.alignment = TextAnchor.MiddleCenter;

		GUIContent content = new GUIContent(node.text);
		textAreaSize = style.CalcSize(content);

		EditorGUILayout.BeginHorizontal();
		node.text = EditorGUILayout.TextField(node.text);

		if (GUILayout.Button("Crear!"))
		{
			node.AddOption("");
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
