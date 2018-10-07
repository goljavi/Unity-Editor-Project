using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public string textContent;
    public Node parent;
    public List<Node> children;

    public Node(string newTextContent, Node newParent) //Crear nodo hijo o comun
    {
        textContent = newTextContent;
        parent = newParent;
    }

    public Node(string newTextContent) // Crear nodo raiz o inicial
    {
        textContent = newTextContent;
    }

    public void CreateChild(string newTextContent)
    {
        Node child = new Node(newTextContent, this);
    }

}
