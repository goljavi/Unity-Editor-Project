using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class BaseEditorNode : ScriptableObject
{
    public string nodeName = "Base Node";
    public Rect windowRect = new Rect(10, 10, 100, 100);
    public float offsetHeight = 0;
    public float textHeight = 19;
    public float startingWindowHeight = 100;
    public float addedWindowHeight = 0;
    public BaseClientNode node;

    void OnEnable()
    {
        if (nodeName == "Base Node") node = new RootNode();
        else node = new Node("");
    }

    public virtual void DrawWindow()
    {
        addedWindowHeight = 0;
    }

    public virtual void DrawCurve()
    {

    }

    public void MoveNodePosition(Vector2 ammount)
    {
        windowRect.position += ammount;
    }
}



