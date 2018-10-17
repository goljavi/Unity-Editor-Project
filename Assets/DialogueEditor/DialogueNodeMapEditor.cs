using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueNodeMap))]
public class DialogueNodeMapEditor : Editor
{
    private DialogueNodeMap _target;
    DialogueEditor window;

    private void OnEnable()
    {
        _target = (DialogueNodeMap)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Abrir Ventana de Nodos"))
        {
            window = EditorWindow.GetWindow<DialogueEditor>();
            window.minSize = new Vector2(800, 600);
            window.LoadAssetFile(_target);
        }
    }
}
