using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseClientNode
{
}

public class Node : BaseClientNode
{
    public string text;
    public Option parent;
    public List<Option> options = new List<Option>();
	private readonly uint nodeID;
	private static uint nodeCount = 0;

    public Node(string newTextContent) // Crear nodo raiz o inicial
    {
        text = newTextContent;
		nodeID = nodeCount;
		nodeCount++;
    }

    public Node(string newTextContent, Option newParent) //Crear nodo hijo o comun
    {
        text = newTextContent;
        parent = newParent;
		nodeID = nodeCount;
		nodeCount++;
    }



    public void AddOption(string optionText = "")
    {
        var newOption = new Option(this);
        newOption.Text = optionText;
        options.Add(newOption);
    }

	public void ChangeParent(Option newParentOption) {
		parent = newParentOption;
		newParentOption.ChildNode = this;
	}
}

public class RootNode : BaseClientNode
{
    public Node start;

	public RootNode() {
		SetStart(new Node(""));
	}

    public RootNode(Node startingNode)
    {
        SetStart(startingNode);
    }

    public void SetStart(Node startingNode)
    {
        start = startingNode;
    }
}

public class Option
{
	private string text;
	private Node childNode;
	public readonly Node container;

    public Option(Node container)
    {
        text = "";
        childNode = new Node("New node");
		this.container = container;
    }

    public Node ChildNode
    {
		set
		{
			childNode.parent = null;
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
