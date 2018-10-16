using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode {
    public int id;
    public List<BaseNode> parents = new List<BaseNode>();
    public Rect windowRect;
    public string windowTitle;
    public DialogueEditor reference;
	public virtual void DrawNode() { }
    public virtual void DrawConnection() { }

    //Builder
    public virtual BaseNode SetWindowRect(Rect value)
    {
        windowRect = value;
        return this;
    }

    public virtual BaseNode SetWindowTitle(string value)
    {
        windowTitle = value;
        return this;
    }

    public virtual BaseNode SetReference(DialogueEditor value)
    {
        reference = value;
        return this;
    }

    public virtual BaseNode SetId(int value)
    {
        id = value;
        return this;
    }

    public virtual BaseNode SetParent(BaseNode value)
    {
        parents.Add(value);
        return this;
    }
}
