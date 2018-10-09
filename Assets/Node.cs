using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode
{
}

public class Node : BaseNode
{
    public string text;
    public Node parent;
    public List<Option> options = new List<Option>();

    public Node(string newTextContent) // Crear nodo raiz o inicial
    {
        text = newTextContent;
    }

    public Node(string newTextContent, Node newParent) //Crear nodo hijo o comun
    {
        text = newTextContent;
        parent = newParent;
    }



    public void AddOption(string optionText = "")
    {
        var newOption = new Option();
        newOption.text = optionText;
        options.Add(newOption);
    }
}

public class RootNode : BaseNode
{
    public Node start;

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
    public string text;
    public Node linkedNode;

    public Option()
    {
        text = "";
        linkedNode = null;
    }

    public void SetLink(Node newLink)
    {
        linkedNode = newLink;
    }
}
