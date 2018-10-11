using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeTree : MonoBehaviour {

	private Node _currentNode;
	private bool _initialized = false;
	//public NodeTreeFileContainerType data;

	// Update is called once per frame
	void Update() {

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

	public void SelectOption(Option option) {
		if(_currentNode != null && _currentNode.options != null)
		{
			if (_currentNode.options.Contains(option))
			{
				_currentNode = option.ChildNode;
				return;
			} 
			Debug.LogError("Selected option is not inside the current node");
		}
	}

	public string NodeText {
		get
		{
			return _currentNode.text;
		}
	}

	public List<Option> NodeResponces {
		get
		{
			return new List<Option>(_currentNode.options);
		}
	}

}
