using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public string text;
    public Node parent;
    public List<NodeOption> options;


    public Node(string newTextContent, Node newParent) //Crear nodo hijo o comun
    {
        text = newTextContent;
        parent = newParent;
    }

    public Node(string newTextContent) // Crear nodo raiz o inicial
    {
        text = newTextContent;
    }

    public void AddOption()
    {
        var newOption = new NodeOption();
        options.Add(newOption);
    }

}

public class NodeOption
{
    public string text;
    public Node linkedNode;

    public NodeOption()
    {
        text = "";
        linkedNode = null;
    }
}
