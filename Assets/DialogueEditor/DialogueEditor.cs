using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.DialogueEditor
{

    public class DialogueEditor : EditorWindow
    {
        private static List<BaseEditorNode> nodes = new List<BaseEditorNode>();
        private Vector3 mousePosition;
        private bool makeTransition;
        private bool clickOnWindow;
        private BaseEditorNode selectedNode;

        public enum UserActions
        {
            addQuestion,
            addStartingNode,
            addTransitionNode,
            deleteNode
        }

        [MenuItem("Dialogue Editor/Editor")]
        static void ShowEditor()
        {
            DialogueEditor editor = EditorWindow.GetWindow<DialogueEditor>();
            editor.minSize = new Vector2(400, 200);
        }

        void Awake()
        {
            StartNode startingNode = StartNode.CreateInstance<StartNode>();
            {
                startingNode.windowRect = new Rect(0, 0, 100, 100);
                startingNode.nodeName = "Starting Node";
            }

            nodes.Add(startingNode);
        }

        void OnDestroy()
        {
            nodes.Clear();
        }

        private void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
        }

        void DrawWindows()
        {
            BeginWindows();
            foreach (BaseEditorNode n in nodes)
            {
                n.DrawCurve();
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].windowRect = GUI.Window(i, nodes[i].windowRect, DrawNodeWindow, nodes[i].nodeName);
            }

            EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            nodes[id].DrawWindow();
            GUI.DragWindow();
        }

        void UserInput(Event e)
        {
            if(e.button == 1  && !makeTransition)
            {
                if(e.type == EventType.MouseDown)
                {
                    RightClick(e);
                }
            }

            if (e.button == 0 && !makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                    LeftClick(e);
                }
            }
        }

        void RightClick(Event e)
        {
            selectedNode = null;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].windowRect.Contains(e.mousePosition))
                {
                    clickOnWindow = true;
                    selectedNode = nodes[i];
                    break;
                }
            }

            if (clickOnWindow)
                ModifyNode(e);


            clickOnWindow = false;
        }




        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();

            if (selectedNode is QuestionNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
            }

            if (selectedNode is StartNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Add Question"), false, ContextCallback, UserActions.addQuestion);
            }

            menu.ShowAsContext();
            e.Use();
        }

        public void ContextCallback(object o)
        {
            UserActions a = (UserActions)o;
            switch (a)
            {
                case UserActions.addQuestion:

					QuestionNode questionNode = QuestionNode.CreateInstance<QuestionNode>();
					questionNode.node.ChangeParent(selectedNode.node.AddOption("", questionNode.node));
                    questionNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 400, 200);
                    questionNode.nodeName = "Question";
                    nodes.Add(questionNode);


                    break;
                case UserActions.deleteNode:
                    if (selectedNode != null)
                    {
                        nodes.Remove(selectedNode);
                    }
                    break;
            }
        }

        

        void LeftClick(Event e)
        {

        }
    }

}
