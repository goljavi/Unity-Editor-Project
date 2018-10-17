using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* Este es el custom editor de los archivos "DialogueNodeMap"
 * Sirve para poder abrir la ventana de nodos haciendo click en el archivo */
[CustomEditor(typeof(DialogueNodeMap))]
public class DialogueNodeMapEditor : Editor
{
    private DialogueNodeMap _target;
    DialogueEditor window;

    private void OnEnable()
    {
        //Cargo la referencia al archivo
        _target = (DialogueNodeMap)target;
    }

    public override void OnInspectorGUI()
    {
        //Muestro el botón para abrir la ventana de nodos en el editor del archivo
        if (GUILayout.Button("Abrir Ventana de Nodos"))
        {
            //Abro la ventana de nodos
            window = EditorWindow.GetWindow<DialogueEditor>();

            //Le seteo su tamaño
            window.minSize = new Vector2(800, 600);

            //Le paso la referencia del archivo a la ventana de nodos
            window.LoadAssetFile(_target);
        }
    }
}
