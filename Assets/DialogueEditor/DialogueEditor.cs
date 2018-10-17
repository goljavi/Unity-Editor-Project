using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* Este script que se encarga de recibir el archivo de tipo "DialogueNodeMap", deserializarlo, 
 * mostrar la interfaz de nodos, permitir la edición de los mismos y la serialización al guardar los mismos.
 */
public class DialogueEditor : EditorWindow {

    #region VARIABLES
    //Acá se guardan los nodos que se muestran en la ventana. Esta lista se muestra en cada OnGUI() -> DrawNodes()
    List<BaseNode> _nodes = new List<BaseNode>();

    //Acá se guarda la posición del mouse dentro de la ventana. Se guarda en cada OnGUI()
    Vector3 _mousePosition;

    /* Acá se guardan los ultimo nodos en los cual se hizo click derecho y click izquierdo (según corresponde)
     * Esto sirve para poder identificar que nodo quiso seleccionar la persona a la hora de realizar una acción
     * Se guardan en la función OnGUI() -> UserInput()
     */
    BaseNode _lastRightClickedNode;
    BaseNode _lastLeftClickedNode;

    //Contiene la referencia al archivo en el cual se está serializando la información
    DialogueNodeMap _assetFile;

    /* Contiene una lista con todos los ID de todos los nodos, generados al azar. 
     * Se usa unicamente con la intención de que no se repitan nunca los ID asignados a los nodos.
     * Se le asigand ID's a los nodos para poder serializarlos, ya que el sistema de unity no permite
     * Que un nodo hijo contenga a su nodo padre (podria darse una recursión infinita)
     * Se actualiza con la función GetNewId() 
     */
    List<int> _idList = new List<int>();

    //En este enum están todas las posibles acciones a las que se puede llamar
    //haciendo click derecho en el editor ya sea en un nodo individual o no.
    public enum UserActions
    {
        addStartNode,
        addEndNode,
        addDialogueNode,
        addOptionNode,
        deleteNode,
        addConnection
    }
    #endregion

    #region SERIALIZADO E INTERPRETE
    /* En esta función se deserealiza el archivo "DialogueNodeMap". Se llama desde el archivo
     * DialogueNodeMapEditor Ese archivo se encarga de mostrar el botón para abrir la ventana de 
     * nodos de ese archivo en particular, por lo tanto es el que contiene la referencia al archivo.
     * En esta función se agarra la lista de "DialogueMapSerializedObject" que contiene cada nodo en
     * un modo que no es legible por el editor de nodos pero que sirve para que le guste al sistema de
     * serializado de unity (el sistema no admite clases abstractas ni recursión). Esta función hace de 
     * interprete, pasando de DialogueMapSerializedObject a el tipo de nodo que corresponda 
     * (basenode, startnode, endnode, etc)
     */
    public void LoadAssetFile(DialogueNodeMap assetFile)
    {
        _assetFile = assetFile;

        //Se borran las listas en caso de que haya información anterior no deseada
        _nodes.Clear();
        _idList.Clear();

        //Interpreto en base al título que clase de nodo se guardó y lo genero en la ventana
        foreach(var map in assetFile.nodes)
        {
            //Guardo el ID de cada uno en la lista de ID's, para que al crear nuevos no se superpongan
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

        /* Una vez que están todos creados por separado les asigno sus padres a cada uno
         * Por cada "DialogueMapSerializedObject" (detro de el archivo serializado)
         */
        foreach (var map in assetFile.nodes)
        {
            //Por cada Nodo (no serializado, sino dentro del editor)
            foreach (var node in _nodes)
            {
                /* Busco la coincidencia, es decir que estoy parado en el mismo nodo
                 * tanto en el foreach de DialogueMapSerializedObject como en el de los Nodos
                 */
                if (node.id == map.id)
                {
                    /* Si hay coincidencia recorro cada parentId del DialogueMapSerializedObject
                     * Ya que lo necesito para luego buscar la coincidencia entre el ID y el nodo 
                     * generado y así finalmente asignarle el padre a su hijo
                     */
                    foreach (var parentId in map.parentIds)
                    {
                        /* Vuelvo a hacer un recorrido de cada uno de los nodos ya 
                         * generados para encontrar el nodo que contenga el parentId
                         */
                        foreach (var n in _nodes)
                        {
                            if (n.id == parentId)
                            {
                                /* Si hubo coincidencia seteo al nodo que se encontró recorriendo los parentId del objeto serializado
                                 * como padre del nodo que se encontró recorriendo los id del objeto serializado
                                 */
                                node.SetParent(n);
                            }
                        }
                    }
                }
            }
        }

    }

    //Acá se convierte cada Nodo (guardado en la variable _nodes) a "DialogueMapSerializedObject" 
    //y se guarda en una lista en el objeto serializado (de tipo "DialogueNodeMap")
    public void SaveAssetFile()
    {
        //Borro cualquier información previamente guardada en el archivo
        _assetFile.nodes.Clear();

        //Por cada nodo
        foreach (var node in _nodes)
        {
            //Genero una lista de ID's de los padres del nodo
            List<int> parentsIds = new List<int>();
            foreach(var parent in node.parents)
            {
                if(parent != null) parentsIds.Add(parent.id);
            }

            //Genero el DialogueMapSerializedObject y lo agrego a la lista
            _assetFile.nodes.Add(new DialogueMapSerializedObject() { id = node.id, parentIds = parentsIds, windowRect = node.windowRect, windowTitle = node.windowTitle });
        }

        //Esto no sé bien que hace pero se solucionó un bug usandolo.
        EditorUtility.SetDirty(_assetFile);
    }
    #endregion

    #region DIBUJADO DE LOS NODOS Y REGISTRO DE INPUT
    //Es el update del EditorWindow
    private void OnGUI()
    {
        //Logeo la posición del mouse
        Event e = Event.current;
        _mousePosition = e.mousePosition;

        //Registro si hizo click izquierdo o derecho
        UserInput(e);

        //Dibujo los nodos sobre la ventana
        DrawNodes();

        //Guardo la información registrada hasta el momento
        SaveAssetFile();
    }

    //Se encarga de dibujar los nodos sobre la ventana
    void DrawNodes()
    {
        BeginWindows();

        /* Cada tipo de nodo puede tener su preferencia de como dibujar las conexiones entre el y su padre
         * así que recorro todos los nodos y les pido a cada uno que se encargue de dibujar la conexión entre el y su padre
         */
        foreach (BaseNode n in _nodes)
        {
            n.DrawConnection();
        }

        //Dibujo el nodo sobre la ventana. Le seteo id, Rect, Title y seteo 
        //a DrawNodeWindow como la función para dibujar las cosas internas
        for (int i = 0; i < _nodes.Count; i++)
        {
            _nodes[i].windowRect = GUI.Window(i, _nodes[i].windowRect, DrawNodeWindow, _nodes[i].windowTitle);
        }

        EndWindows();
    }

    //Esta función dibuja el contenido interno del nodo
    void DrawNodeWindow(int id)
    {
        /* Cada tipo de nodo puede tener su preferencia de mostrar dentro del mismo
         * así que le pido al nodo que se encargue de dibujar y mostrar su contenido
         */
        _nodes[id].DrawNode();

        //Esta función hace que el nodo se pueda mover con el mouse
        GUI.DragWindow();
    }

    //Registra el input del mouse del user
    void UserInput(Event e)
    {
        //Si el evento fue de tipo "MouseDown (click)
        if (e.type == EventType.MouseDown)
        {
            var clickedOnNode = false;

            //Por cada nodo mostrado en ventana
            for (int i = 0; i < _nodes.Count; i++)
            {
                //Si el mouse se encontraba en el rectangulo del nodo
                if (_nodes[i].windowRect.Contains(e.mousePosition))
                {
                    //Hizo click en un nodo!
                    clickedOnNode = true;

                    //Si hizo click izquierdo
                    if (e.button == 0)
                    {
                        //Logeo a ese nodo como el ultimo en el que se hizo click izquierdo
                        _lastLeftClickedNode = _nodes[i];
                    }
                    //Si hizo click derecho
                    else if (e.button == 1)
                    {
                        //Logeo a ese nodo como el ultimo en el que se hizo click derecho
                        _lastRightClickedNode = _nodes[i];
                    }
                    break;
                }
            }

            //Si hizo click derecho llamo a la función RightClick
            if(e.button == 1)
            {
                RightClick(e, clickedOnNode);
            }
        }
    }

    //Esta función se encarga de llamar a las funciones que hacen los menues contextuales
    void RightClick(Event e, bool clickedOnWindow)
    {
        if (clickedOnWindow)
        {
            //Si clickeó en un nodo, el menú contextual al que debe llamar es el de editar nodo
            ModifyNode(e);
        }
        else
        {
            AddNewNode(e);
        }
    }

    //Esto se explica por si solo
    void AddNewNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Start"), false, ContextMenuActions, UserActions.addStartNode);
        menu.AddItem(new GUIContent("Add Dialogue"), false, ContextMenuActions, UserActions.addDialogueNode);
        menu.AddItem(new GUIContent("Add End"), false, ContextMenuActions, UserActions.addEndNode);
        menu.ShowAsContext();
        e.Use();
    }

    void ModifyNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        if(_lastRightClickedNode is DialogueNode)
        {
            menu.AddItem(new GUIContent("Add Option"), false, ContextMenuActions, UserActions.addOptionNode);
            menu.AddItem(new GUIContent("Add Connection (with focused node)"), false, ContextMenuActions, UserActions.addConnection);
            menu.AddItem(new GUIContent("Delete"), false, ContextMenuActions, UserActions.deleteNode);
        }
        else if (_lastRightClickedNode is OptionNode)
        {
            menu.AddItem(new GUIContent("Add Dialogue"), false, ContextMenuActions, UserActions.addDialogueNode);
            menu.AddItem(new GUIContent("Add Connection (with focused node)"), false, ContextMenuActions, UserActions.addConnection);
            menu.AddItem(new GUIContent("Delete"), false, ContextMenuActions, UserActions.deleteNode);
        }
        else if (_lastRightClickedNode is StartNode)
        {
            menu.AddItem(new GUIContent("Add Connection (with focused node)"), false, ContextMenuActions, UserActions.addConnection);
            menu.AddItem(new GUIContent("Add Dialogue"), false, ContextMenuActions, UserActions.addDialogueNode);
            menu.AddItem(new GUIContent("Delete"), false, ContextMenuActions, UserActions.deleteNode);
        }
        else if (_lastRightClickedNode is EndNode)
        {
            menu.AddItem(new GUIContent("Delete"), false, ContextMenuActions, UserActions.deleteNode);
        }

        menu.ShowAsContext();
        e.Use();
    }
    #endregion

    #region CREADO Y BORRADO DE NODOS / CONEXIONES ENTRE NODOS

    //Esta función es llamada por los items de los menues contextuales
    void ContextMenuActions(object o)
    {
        //Como GenericMenu.AddItem() pide una función que devuelva void y reciba un object, 
        //hay que upcastear de object al tipo de variable u objeto que estas queriendo usar.
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
                DeleteNode();
                break;
            default:
                break;
        }
    }

    public void DeleteNode()
    {
        var target = _lastRightClickedNode;
        RemoveParentReferencesInChildNodes(target);
        _idList.Remove(target.id);
        _nodes.Remove(target);
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
    #endregion
}
