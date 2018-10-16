using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OptionNode : BaseNode {
    public string text;

    public override void DrawNode()
    {
        text = EditorGUILayout.TextArea(text, GUILayout.Height(80));
    }

    public override void DrawConnection()
    {
        if (parents.Count > 0)
        {
            foreach(var parent in parents)
            {
                if (parent != null) DialogueEditor.DrawNodeConnection(parent.windowRect, windowRect, true, Color.black);
            }
        }
    }
}
