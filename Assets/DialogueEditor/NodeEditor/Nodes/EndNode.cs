using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndNode : BaseNode {
	public override string GetNodeType { get { return "End"; } }

	public override bool CanTransitionTo(BaseNode node) {
		return false;
	}
}
