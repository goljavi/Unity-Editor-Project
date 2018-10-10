using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.DialogueEditor
{
    [System.Serializable]
    public abstract class BaseNode : ScriptableObject
    {
        public string nodeName = "Base Node";
        public Rect windowRect = new Rect(10, 10, 100, 100);
        public float offsetHeight = 0;
        public float textHeight = 19;
        public float startingWindowHeight = 100;
        public float addedWindowHeight = 0;


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
}


