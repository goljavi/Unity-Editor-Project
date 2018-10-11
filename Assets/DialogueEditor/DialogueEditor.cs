using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.DialogueEditor
{

    public class DialogueEditor : EditorWindow
    {
        static List<BaseEditorNode> windows = new List<BaseEditorNode>();
        Vector3 mousePosition;
        bool makeTransition;
        bool clickOnWindow;
        BaseEditorNode selectedNode;

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


        private void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
        }

        private void OnEnable()
        {
            StartNode startingNode = StartNode.CreateInstance<StartNode>();
            {
                startingNode.windowRect = new Rect(0, 0, 100, 100);
                startingNode.nodeName = "Starting Node";
            }

            windows.Add(startingNode);

            
        }

        void DrawWindows()
        {
            BeginWindows();
            foreach (BaseEditorNode n in windows)
            {
                n.DrawCurve();
            }

            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].nodeName);
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
                    {
                        questionNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 400, 200);
                        questionNode.nodeName = "Question";
                    }

                    windows.Add(questionNode);

                    break;
                case UserActions.deleteNode:
                    if (selectedNode != null)
                    {
                        windows.Remove(selectedNode);
                    }
                    break;
            }
        }

        

        void LeftClick(Event e)
        {

        }
    }

}
