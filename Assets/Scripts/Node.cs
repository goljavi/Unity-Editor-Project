using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode
{
}

public class Node : BaseNode
{
    public string text;
    public Option parent;
    public List<Option> options = new List<Option>();
	private int nodeID;
	private static uint nodeCount = 0;

    public Node(string newTextContent) // Crear nodo raiz o inicial
    {
        text = newTextContent;
    }

    public Node(string newTextContent, Option newParent) //Crear nodo hijo o comun
    {
        text = newTextContent;
        parent = newParent;
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

public class RootNode : BaseNode
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
	private readonly Node container;

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
