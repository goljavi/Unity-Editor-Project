﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StartNode : BaseNode, INeedsChildren {
	public override string GetNodeType { get { return "Start"; } }
	private BaseNode childNode;
	private int childId;

	public void AssignChild(BaseNode child, int childPosition) {
		childNode = child;
		childId = child.id;
	}

	public override bool CanTransitionTo(BaseNode node) {
		List<string> types = new List<string> { "Comparison", "Dialogue" };

		return types.Contains(node.GetNodeType);
	}

	public bool HasChildren() {
		return (childNode != null);
	}
}
