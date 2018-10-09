using System.Collections;
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
		if(_currentNode != null && _currentNode.options[value] != null)
		{
			_currentNode = _currentNode.options[value].linkedNode;
		}
	}

	public void Initialize(Node startingNode = null) {
		if (_initialized) return;
		if (startingNode != null)
		{
			_currentNode = startingNode;
		} else
		{
			_currentNode = new RootNode().start;
		}
		_initialized = true;
	}


}
