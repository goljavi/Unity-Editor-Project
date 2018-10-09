﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeTree : MonoBehaviour {

	private Node _currentNode;
	private bool _initialized = false;

	// Update is called once per frame
	void Update() {

	}

	public void SelectOption(int value) {

	}

	public void Initialize(Node startingNode = null) {
		if (_initialized) return;
		if (startingNode != null)
		{
			_currentNode = startingNode;
		} else
		{
			_currentNode = new Node("");
		}
		_initialized = true;
	}


}
