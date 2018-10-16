using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : EditorWindow {
    List<BaseNode> _nodes = new List<BaseNode>();
    Vector3 _mousePosition;
    BaseNode _lastRightClickedNode;
    BaseNode _lastLeftClickedNode;
    DialogueNodeMap _assetFile;
    List<int> _idList = new List<int>();

    public enum UserActions
    {
        addStartNode,
        addEndNode,
        addDialogueNode,
        addOptionNode,
        deleteNode,
        addConnection
    }

    public void LoadAssetFile(DialogueNodeMap assetFile)
    {
        _assetFile = assetFile;
        _nodes.Clear();
        _idList.Clear();

        //Crear los nodos
        foreach(var map in assetFile.nodes)
        {
            _idList.Add(map.id);

            if (map.windowTitle == "Start")
            {
                AddStartNode(map.windowRect, map.id);
            }
            else if (map.windowTitle == "End")
            {
                AddEndNode(map.windowRect, map.id);
            }
            else if (map.windowTitle == "Dialogue")
            {
                AddDialogueNode(map.windowRect, map.id);
            }
            else if (map.windowTitle == "Option")
            {
                AddOptionNode(map.windowRect, map.id);
            }
        }

        //Asignarle sus padres
        foreach(var map in assetFile.nodes)
        {
            foreach(var node in _nodes)
            {
                if(node.id == map.id)
                {
                    foreach (var parentId in map.parentIds)
                    {
                        foreach (var n in _nodes)
                        {
                            if (n.id == parentId)
                            {
                                node.SetParent(n);
                            }
                        }
                    }
                }
            }
        }

    }

    public void SaveAssetFile()
    {
        _assetFile.nodes.Clear();
        foreach (var node in _nodes)
        {
            List<int> parentsIds = new List<int>();
            foreach(var parent in node.parents)
            {
                if(parent != null) parentsIds.Add(parent.id);
            }

            _assetFile.nodes.Add(new DialogueMapSerializedObject() { id = node.id, parentIds = parentsIds, windowRect = node.windowRect, windowTitle = node.windowTitle });
        }

        EditorUtility.SetDirty(_assetFile);
    }

    //Es el update del EditorWindow
    private void OnGUI()
    {
        //Logeo la posición del mouse
        Event e = Event.current;
        _mousePosition = e.mousePosition;
        UserInput(e);
        DrawWindows();
        SaveAssetFile();
    }

    void DrawWindows()
    {
        BeginWindows();
        foreach (BaseNode n in _nodes)
        {
            n.DrawConnection();
        }

        for (int i = 0; i < _nodes.Count; i++)
        {
            _nodes[i].windowRect = GUI.Window(i, _nodes[i].windowRect, DrawNodeWindow, _nodes[i].windowTitle);
        }
        EndWindows();
    }

    void DrawNodeWindow(int id)
    {
        _nodes[id].DrawNode();
        GUI.DragWindow();
    }

    void UserInput(Event e)
    {
        if (e.type == EventType.MouseDown)
        {
            var clickedOnWindow = false;
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].windowRect.Contains(e.mousePosition))
                {
                    clickedOnWindow = true;
                    if (e.button == 0)
                    {
                        _lastLeftClickedNode = _nodes[i];
                    }else if(e.button == 1)
                    {
                        _lastRightClickedNode = _nodes[i];
                    }
                    break;
                }
            }


            if(e.button == 1)
            {
                RightClick(e, clickedOnWindow);
            }
        }
    }

    void RightClick(Event e, bool clickedOnWindow)
    {
        if (clickedOnWindow)
        {
            ModifyNode(e);
        }
        else
        {
            AddNewNode(e);
        }
    }

    void AddNewNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Start"), false, ContextCallback, UserActions.addStartNode);
        menu.AddItem(new GUIContent("Add Dialogue"), false, ContextCallback, UserActions.addDialogueNode);
        menu.AddItem(new GUIContent("Add End"), false, ContextCallback, UserActions.addEndNode);
        menu.ShowAsContext();
        e.Use();
    }

    void ModifyNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        if(_lastRightClickedNode is DialogueNode)
        {
            menu.AddItem(new GUIContent("Add Option"), false, ContextCallback, UserActions.addOptionNode);
            menu.AddItem(new GUIContent("Add Connection (with focused node)"), false, ContextCallback, UserActions.addConnection);
            menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
        }
        else if (_lastRightClickedNode is OptionNode)
        {
            menu.AddItem(new GUIContent("Add Dialogue"), false, ContextCallback, UserActions.addDialogueNode);
            menu.AddItem(new GUIContent("Add Connection (with focused node)"), false, ContextCallback, UserActions.addConnection);
            menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
        }
        else if (_lastRightClickedNode is StartNode)
        {
            menu.AddItem(new GUIContent("Add Connection (with focused node)"), false, ContextCallback, UserActions.addConnection);
            menu.AddItem(new GUIContent("Add Dialogue"), false, ContextCallback, UserActions.addDialogueNode);
            menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
        }
        else if (_lastRightClickedNode is EndNode)
        {
            menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
        }

        menu.ShowAsContext();
        e.Use();
    }

    void ContextCallback(object o)
    {
        UserActions a = (UserActions)o;
        switch (a)
        {
            case UserActions.addStartNode:
                AddStartNode(new Rect(_mousePosition.x, _mousePosition.y, 100, 100), GetNewId());
                break;
            case UserActions.addDialogueNode:
                AddDialogueNode(new Rect(_mousePosition.x, _mousePosition.y, 200, 130), GetNewId(), _lastRightClickedNode);
                break;
            case UserActions.addOptionNode:
                AddOptionNode(new Rect(_mousePosition.x, _mousePosition.y, 200, 130), GetNewId(), (DialogueNode)_lastRightClickedNode);
                break;
            case UserActions.addEndNode:
                AddEndNode(new Rect(_mousePosition.x, _mousePosition.y, 100, 100), GetNewId());
                break;
            case UserActions.addConnection:
                AddConnection();
                break;
            case UserActions.deleteNode:
                var target = _lastRightClickedNode;
                RemoveParentReferencesInChildNodes(target);
                _idList.Remove(target.id);
                _nodes.Remove(target);
                break;
            default:
                break;
        }
    }

    public void AddConnection()
    {
        if (_lastRightClickedNode == null || _lastLeftClickedNode == null) return;
        if (_lastLeftClickedNode is StartNode && _lastRightClickedNode is DialogueNode
            || _lastLeftClickedNode is DialogueNode && _lastRightClickedNode is OptionNode
            || _lastLeftClickedNode is OptionNode && _lastRightClickedNode is DialogueNode
            || _lastLeftClickedNode is EndNode && _lastRightClickedNode is OptionNode)
        {
            _lastRightClickedNode.SetParent(_lastLeftClickedNode);
        }   
    }

    public StartNode AddStartNode(Rect rect, int id)
    {
        StartNode startNode = new StartNode();
        startNode.SetWindowRect(rect).SetWindowTitle("Start").SetId(id).SetReference(this);
        _nodes.Add(startNode);
        return startNode;
    }

    public EndNode AddEndNode(Rect rect, int id)
    {
        EndNode endNode = new EndNode();
        endNode.SetWindowRect(rect).SetWindowTitle("End").SetId(id).SetReference(this);
        _nodes.Add(endNode);
        return endNode;
    }

    public DialogueNode AddDialogueNode(Rect rect, int id, BaseNode parent = null)
    {
        DialogueNode dialogueNode = new DialogueNode();
        dialogueNode.SetWindowRect(rect).SetWindowTitle("Dialogue").SetParent(parent).SetId(id).SetReference(this);
        _nodes.Add(dialogueNode);
        return dialogueNode;
    }

    public OptionNode AddOptionNode(Rect rect, int id, DialogueNode parent = null)
    {
        OptionNode optionNode = new OptionNode();
        optionNode.SetWindowRect(rect).SetWindowTitle("Option").SetParent(parent).SetId(id).SetReference(this);
        _nodes.Add(optionNode);
        return optionNode;
    }

    public static void DrawNodeConnection(Rect start, Rect end, bool left, Color curveColor)
    {
        Handles.DrawLine(start.center, end.center);
    }

    public void RemoveParentReferencesInChildNodes(BaseNode target)
    {
        var childNodes = GetChildNodes(target);
        foreach (var node in childNodes)
        {
            node.parents.Remove(target);
        }
    }

    public List<BaseNode> GetChildNodes(BaseNode n)
    {
        List<BaseNode> childs = new List<BaseNode>();
        foreach (var node in _nodes)
        {
            foreach (var parent in node.parents)
            {
                if (parent == n)
                {
                    childs.Add(node);
                }
            }
        }

        return childs;
    }

    public int GetNewId()
    {
        int randomNumber;
        do{ randomNumber = Random.Range(0, 10000); } while (_idList.Contains(randomNumber));
        _idList.Add(randomNumber);
        return randomNumber;
    }
}
