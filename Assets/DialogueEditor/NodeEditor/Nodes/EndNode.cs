using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndNode : BaseNode {
	public override string GetNodeType { get { return "End"; } }

	public override bool CanTransitionTo(BaseNode node) {
		return false;
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
}
