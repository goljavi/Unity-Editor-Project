using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Este script se encarga de crear un nuevo item en el AssetMenu de Unity para crear un tipo de archivo "DialogueNodeMap"
 * Este archivo almacena la lista de nodos que se va a cargar en el editor de nodos de forma serializada */
[CreateAssetMenu(fileName = "New Dialogue Map", menuName = "Dialogue Map")]
public class DialogueNodeMap : ScriptableObject
{
    public List<DialogueMapSerializedObject> nodes = new List<DialogueMapSerializedObject>();
}