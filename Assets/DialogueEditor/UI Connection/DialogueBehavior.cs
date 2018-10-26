using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Este script que se encarga de recibir el archivo de tipo "DialogueNodeMap" 
 * y hacer de interfaz entre el sistema de nodos (servicio) y el script del programador (cliente) */
public class DialogueBehavior : MonoBehaviour
{
    //Uso un serializefield para que unity muestre el campo para que diseñador dropee su archivo de mapa de nodos
    [SerializeField]
    private DialogueNodeMap dialog;

    //Aca guardo el primer "DialogueObject" que haya, es decir, el primer dialogo que apunta al Start
    private DialogueObject first;

    /* En esta lista se guardan los objetos "DialogueObject" que se consiguen procesando la 
     * información de los nodos y es lo que se le entrega al programador para armar la UI */
    private List<DialogueObject> dialogueObjects;

    //Acá se guarda la referencia de una opción al siguiente DialogueObject
    private Dictionary<int, DialogueObject> optionIdToNextDialogue;

    void Start()
    {
        dialogueObjects = new List<DialogueObject>();
        optionIdToNextDialogue = new Dictionary<int, DialogueObject>();
        int start = 0;
        foreach (var node in dialog.nodes)
        {
            if (node.windowTitle == "Start")
            {
                //Si el titulo del nodo es "Start" guardo su id
                start = node.id;
            }
            else if (node.windowTitle == "Dialogue")
            {
                //Si el nodo es de tipo "Dialogue" lo convierto en "DialogueObject" y lo agrego a la lista de dialogueObjects
                dialogueObjects.Add(GetDialogueObject(node));
            }
        }

        //Genero las referencias entre las opciones y los siguientes dialogos
        GenerateIdToNextDictionary();

        //En base al id del nodo "start" guardo el primer dialogo
        GetFirstDialogue(start);
    }

    //Devuelve el primer dialogo
    public DialogueObject GetStartingDialogue()
    {
        return first;
    }

    /* Devuelve el dialogo que le sigue al anterior, en base al optionId que haya elegido 
     * el usuario. También devuelve null si no hay mas dialogo o si se llegó al final */
    public DialogueObject GetNextDialogue(int optionId)
    {
        if (!optionIdToNextDialogue.ContainsKey(optionId)) return null;
        return optionIdToNextDialogue[optionId];
    }

    //Genera un diccionario que contiene el ID de la opción y el ID del DialogueObject al que linkea esa opción
    void GenerateIdToNextDictionary()
    {
        //Por cada DialogueObject
        foreach (var diagObj in dialogueObjects)
        {
            //Por cada opción que tenga ese DialogueObject
            foreach (var option in diagObj.optionsLinks)
            {
                foreach (var diagObj2 in dialogueObjects)
                {
                    //Busco la coincidencia entre el id del DialogueObject y el id de la referencia del option
                    if (diagObj2.id == option.Value) optionIdToNextDialogue[option.Key] = diagObj2;
                }
            }
        }
    }

    //Busca el primer dialogo en el mapa de nodos
    DialogueObject GetFirstDialogue(int startId)
    {
        //Por cada nodo
        foreach (var node in dialog.nodes)
        {
            //Por cada parent de cada nodo
            foreach (var parentId in node.parentIds)
            {
                //Si hay una coincidencia entre el parentId y el startId singifica que el nodo tiene como padre al StartingNode
                if (parentId == startId)
                {
                    /* Ahora que tengo el id del primer dialogo necesito encontrarlo en la 
                     * lista de dialogueObjects para asignarlo como el primer dialogo */
                    foreach (var dialogue in dialogueObjects)
                    {
                        if (node.id == dialogue.id)
                        {
                            first = dialogue;
                            return first;
                        }
                    }
                }
            }
        }

        return null;
    }

    //Convierte de DialogueMapSerializedObject a DialogueObject, que es el objeto que se le entrega al usuario final
    DialogueObject GetDialogueObject(DialogueMapSerializedObject dialogueNode)
    {
        /* Guardo el id y el dialogo
         * Las opciones se las pido a GetDialogueOptions que me entrega una Diccionario |Key: id del option| |Value: el texto del option| */
        var dialogueObj = new DialogueObject
        {
            id = dialogueNode.id,
            dialogue = dialogueNode.data,
            options = GetDialogueOptions(dialogueNode)
        };

        /* Una vez que tengo el diccionario de opciones creado pido el linkeo entre una opción y el siguiente nodo a GetOptionsLinks()
         * Que me devuelve |Key: id del option| |Value: id del nodo al que linkea| */
        dialogueObj.optionsLinks = GetOptionsLinks(dialogueObj.options);
        return dialogueObj;
    }

    //Devuelve un diccionario con el id de las opciones y texto de un nodo especifico
    Dictionary<int, string> GetDialogueOptions(DialogueMapSerializedObject dialogueNode)
    {
        var options = new Dictionary<int, string>();

        //Por cada nodo
        foreach (var node in dialog.nodes)
        {
            //Si el nodo es de tipo "Option"
            if (node.windowTitle == "Option")
            {
                //Por cada parentId
                foreach (var parentId in node.parentIds)
                {
                    /* Si hay coincidencia entre el parentId y el id del dialogueNode
                     * Significa que esa opción tiene como padre al nodo de dialogo */
                    if (parentId == dialogueNode.id) options.Add(node.id, node.data);
                }
            }
        }

        return options;
    }

    //Devuelve un diccionario con los id de las options de un DialogueObject y los id de los DialogueObject a los que esas options vinculan
    public Dictionary<int, int> GetOptionsLinks(Dictionary<int, string> options)
    {
        var optionsLinks = new Dictionary<int, int>();

        //Por cada opción de la lista de opciones
        foreach (var option in options)
        {
            //Por cada nodo
            foreach (var node in dialog.nodes)
            {
                //Por cada padre del nodo
                foreach (var parentId in node.parentIds)
                {
                    /* Si hay coicidencia entre el parentId del nodo y el id de la opción
                     * Significa que ese nodo tiene de padre a esa opción */
                    if (option.Key == parentId) optionsLinks.Add(option.Key, node.id);
                } 
            }
        }
        return optionsLinks;
    }
}