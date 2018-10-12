﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode {

	public string title;
	public readonly uint nodeID;
	private static uint nodeCount = 0;

	public static uint NodeCount {
		get
		{
			return nodeCount;
		}

		set
		{
			nodeCount = value;
		}
	}

	public BaseNode() {
		nodeID = BaseNode.nodeCount;
		nodeCount++;
	}

	public abstract Option AddOption(string optionText, Node attatchedNode);
}

public class Node : BaseNode {

	public string text;
	private Option parent;
	public List<Option> options = new List<Option>();

	public Option Parent {
		get
		{
			return parent;
		}
		set
		{
			parent = value;
			if(value!=null)
				value.ChildNode = this;
		}
	}

	public Node(string newTextContent) // Crear nodo raiz o inicial
	{
		text = newTextContent == "" ? nodeID.ToString() : newTextContent;
	}

	public Node(string newTextContent, Option newParent) //Crear nodo hijo o comun
	{
		text = newTextContent == "" ? nodeID.ToString() : newTextContent;
		parent = newParent;
	}

	public override Option AddOption(string optionText, Node attachedNode = null) {
		var newOption = new Option(this);
		newOption.Text = optionText;
		options.Add(newOption);
		if (attachedNode!=null)
			newOption.ChildNode = attachedNode;
		return newOption;
	}

	public void ChangeParent(Option newParentOption) {
		parent = newParentOption;
		if(newParentOption!=null)
			newParentOption.ChildNode = this;
	}
}

public class RootNode : BaseNode {
	public Node start;
	public Option rootOption;

	public RootNode() {
		//SetStart(new Node(""));
		rootOption = new Option(this);
		title = "Root";
	}

	public RootNode(Node startingNode) {
		SetStart(startingNode);
	}

	public void SetStart(Node startingNode) {
		start = startingNode;
	}

	public override Option AddOption(string optionText, Node attachedNode) {
		var newOption = new Option(this);
		newOption.Text = optionText;
		newOption.ChildNode = attachedNode;
		rootOption = newOption;
		return newOption;
	}
}

public class Option {
	private string text;
	private Node childNode;
	public readonly BaseNode container;

	public Option(BaseNode container) {
		text = "";
		childNode = new Node("New node");
		this.container = container;
	}

	public Node ChildNode {
		set
		{
			childNode.Parent = null;
			childNode = value;
		}
		get
		{
			return childNode;
		}
	}

	public string Text {
		get
		{
			return text;
		}
		set
		{
			text = value;
		}
	}
}
