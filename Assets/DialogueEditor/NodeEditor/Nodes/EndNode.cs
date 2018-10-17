using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndNode : BaseNode {
    public override void DrawConnection()
    {
        if (parents.Count > 0)
        {
            foreach (var parent in parents)
            {
                if (parent != null) DialogueEditor.DrawNodeConnection(parent.windowRect, windowRect, true, Color.black);
            }
        }
    }
}
