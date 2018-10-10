using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SA;

namespace SA.DialogueEditor
{
    public class QuestionNode : BaseNode
    {

        /*public string text = "";
        public List<string> replies = new List<string>();
        private Vector2 textAreaSize = new Vector2(0, 0);

        public QuestionNode()
        {
            windowRect = new Rect(0,0,300,100);
            nodeName = "Chat";
        }
        */
        public override void DrawWindow()
        {
           /* base.DrawWindow();
            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.MiddleCenter;

            GUIContent content = new GUIContent(text);
            textAreaSize = style.CalcSize(content);

            text = GUI.TextArea(new Rect(5, offsetHeight, windowRect.width - 10, textAreaSize.y), text);
            addedWindowHeight = textAreaSize.y;

            offsetHeight += addedWindowHeight + 10;

            for (int i = 0; i < replies.Count; i++)
            {
                if (GUI.Button(new Rect(5, offsetHeight, 20, textHeight), "-"))
                {
                    replies.RemoveAt(i);
                    break;
                }

                replies[i] = GUI.TextField(new Rect(30, offsetHeight, windowRect.width - 35, textHeight), replies[i]);
                offsetHeight += textHeight;
                addedWindowHeight += textHeight;
            }
            offsetHeight += 10;

            if (GUI.Button(new Rect(windowRect.width -90, offsetHeight, 80, textHeight), "Add Reply"))
            {
                replies.Add("");
            }

            windowRect.height = startingWindowHeight + addedWindowHeight;
            */
        }
        
        public override void DrawCurve()
        {

        }
    }
}

