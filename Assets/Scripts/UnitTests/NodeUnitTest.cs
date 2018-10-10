﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUnitTest : MonoBehaviour {

    private void Start()
    {
        Test();
    }

    public void Test()
    {
        var node0 = new Node("Node 0");
        var root = new RootNode(node0);
        node0.AddOption("node0.option1 -> node 1");
        node0.AddOption("node0.option2 -> node 2");
        node0.AddOption("node0.option3 -> node 3");
        node0.options[0].ChildNode.AddOption("node1.option1");
        Debug.Log(node0.text);
        foreach (var option in root.start.options)
        {
            Debug.Log(option.Text);
            foreach (var option2 in option.ChildNode.options)
            {
                Debug.Log(option2.Text);
                Debug.Log(option2.ChildNode.text);
            }
        }
    }
}
