using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SA;

namespace SA.DialogueEditor
{
    public class QuestionNode : BaseEditorNode
    {

        public string text = "";
        public List<string> replies = new List<string>();
        private Vector2 textAreaSize = new Vector2(0, 0);

        public QuestionNode()
        {
            windowRect = new Rect(0,0,300,100);
            nodeName = "Chat";
        }
        
        public override void DrawWindow()
        {
            base.DrawWindow();
            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.MiddleCenter;

            GUIContent content = new GUIContent(text);
            textAreaSize = style.CalcSize(content);

            EditorGUILayout.BeginHorizontal();
            text = EditorGUILayout.TextField(text);

            if (GUILayout.Button("Crear!"))
            {
                replies.Add("");
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < replies.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-"))
                {
                    replies.RemoveAt(i);
                    break;
                }

                replies[i] = EditorGUILayout.TextField(replies[i]);
                EditorGUILayout.EndHorizontal();
            }

            
            
        }
        
        public override void DrawCurve()
        {

        }
    }
}

