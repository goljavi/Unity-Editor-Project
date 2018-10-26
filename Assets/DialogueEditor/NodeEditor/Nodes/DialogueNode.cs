using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : BaseNode {
    public string text;


    public override void DrawNode()
    {
        EditorStyles.textArea.wordWrap = true;
        text = EditorGUILayout.TextArea(text, EditorStyles.textArea ,GUILayout.Height(80));      
    }

    public override Color GetBackgroundColor()
    {
        windowColor = Color.green;
        return windowColor;
    }

    public override string GetNodeData()
    {
        return text;
    }

    public override void SetNodeData(string data)
    {
        text = data;
    }

    public override void DrawConnection()
    {
        if (parents.Count > 0)
        {
            foreach (var parent in parents)
            {
                if(parent != null) DialogueEditor.DrawNodeConnection(parent.windowRect, windowRect, true, Color.black);
            }
        }
    }
}
