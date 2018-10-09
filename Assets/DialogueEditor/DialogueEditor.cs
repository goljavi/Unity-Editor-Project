using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.DialogueEditor
{

    public class DialogueEditor : EditorWindow
    {
        static List<BaseNode> windows = new List<BaseNode>();
        Vector3 mousePosition;
        bool makeTransition;
        bool clickOnWindow;
        BaseNode selectedNode;

        public enum UserActions
        {
            addQuestion,
            addAnswer,
            addTransitionNode,
            deleteNode
        }

        [MenuItem("Dialogue Editor/Editor")]
        static void ShowEditor()
        {
            DialogueEditor editor = EditorWindow.GetWindow<DialogueEditor>();
            editor.minSize = new Vector2(400, 200);
        }


        private void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
        }

        private void OnEnable()
        {
            //windows.Clear();
        }

        void DrawWindows()
        {
            BeginWindows();
            foreach (BaseNode n in windows)
            {
                n.DrawCurve();
            }

            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
            }

            EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            windows[id].DrawWindow();
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
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(e.mousePosition))
                {
                    clickOnWindow = true;
                    selectedNode = windows[i];
                    break;
                }
            }

            if (!clickOnWindow)
            {
                AddNewNode(e);
            }

            else
            {
                ModifyNode(e);
            }

            clickOnWindow = false;
        }


        void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add Question"), false, ContextCallback, UserActions.addQuestion);
            menu.AddItem(new GUIContent("Add Starting Node"), false, ContextCallback, UserActions.addAnswer);

            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();

            if (selectedNode is QuestionNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Add Transition"), false, ContextCallback, UserActions.addTransitionNode);
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
            }

            if (selectedNode is StartNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Add Transition"), false, ContextCallback, UserActions.addTransitionNode);
                menu.AddSeparator("");
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
                case UserActions.addQuestion:
                    QuestionNode questionNode = QuestionNode.CreateInstance<QuestionNode>();
                    {
                        questionNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 400, 200);
                        questionNode.windowTitle = "Question";
                    }

                    windows.Add(questionNode);

                    break;
                case UserActions.addAnswer:
                    StartNode startingNode = StartNode.CreateInstance<StartNode>();
                    {
                        startingNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 100, 100);
                        startingNode.windowTitle = "Starting Node";
                    }

                    windows.Add(startingNode);

                    break;
                case UserActions.addTransitionNode:
                    break;
                case UserActions.deleteNode:
                    if (selectedNode != null)
                    {
                        windows.Remove(selectedNode);
                    }
                    break;
                default:
                    break;
            }
        }

        void LeftClick(Event e)
        {

        }
    }

}
