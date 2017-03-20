using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace SpriteMaker
{


    public class DrawCommandManager
    {

        public List<BaseDrawCommand> DrawCommands = new List<BaseDrawCommand>();

        private BaseDrawCommand.DrawCommandType DrawCommandToInsert;

        private int CommandToRemove = -1;
        private int CommandToRearrange = -1;
        private int RearrangeAmount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteMaker.DrawCommandManager"/> class.
        /// </summary>
        public DrawCommandManager()
        {

            DrawFill g = new DrawFill();
            g.blendMode = BaseDrawCommand.BlendMode.Replace;
            DrawCommands.Add(g);
        }

        /// <summary>
        /// just a getter for the draw commands
        /// </summary>
        /// <returns>The draw command queue</returns>
        public List<BaseDrawCommand> GetDrawCommands()
        {
            return DrawCommands;
        }

        /// <summary>
        /// GUI rendering for the Draw Commands
        /// </summary>
        public void DrawControls()
        {


            for (int i = 0; i < DrawCommands.Count; i++)
            {

                //the big box that holds everything
                EditorGUILayout.BeginVertical("Box");
                {


                    //DataGUI is used to draw the top bar of the Draw Command (name,collapse,hide,delete)
                    EditorGUILayout.BeginHorizontal();
                    {
                        DataGUI(i);
                    }
                    EditorGUILayout.EndHorizontal();



                    //When we render the actual command, we make sure its not collapsed
                    if (!DrawCommands[i].Hidden)
                    {

                        EditorGUILayout.BeginHorizontal();
                        {

                            //the actual parameters for each drawcommand are rendered
                            EditorGUILayout.BeginVertical("Box");
                            {
                                DrawCommands[i].DrawControls();
                            }
                            EditorGUILayout.EndVertical();


                            //and then the selection gui that dictate their position in the list is rendered
                            EditorGUILayout.BeginVertical(GUILayout.Width(25));
                            {
                                SelectionGUI(i);
                            }
                            EditorGUILayout.EndVertical();


                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
            }


            //at the end we can insert a new Draw Comma
            EditorGUILayout.BeginHorizontal();
            {
                InsertionGUI(DrawCommands.Count - 1);
            }
            EditorGUILayout.EndHorizontal();

            //after looping through the commands, we run the code that potentially modifies the list in some way
            RemoveDrawCommand(CommandToRemove);
            RearrangeDrawCommand(CommandToRearrange, RearrangeAmount);
            CommandToRemove = -1;
            CommandToRearrange = -1;
            RearrangeAmount = 0;

        }


        /// <summary>
        /// Inserts a new draw command.
        /// </summary>
        /// <param name="_index">Index.</param>
        public void InsertDrawCommand(int _index)
        {
            BaseDrawCommand bdc;
            switch (DrawCommandToInsert)
            {
                case BaseDrawCommand.DrawCommandType.Fill:
                    bdc = new DrawFill();
                    break;
                case BaseDrawCommand.DrawCommandType.Circle:
                    bdc = new DrawCircle();
                    break;
                case BaseDrawCommand.DrawCommandType.Rect:
                default:
                    bdc = new DrawRect();
                    break;
                case BaseDrawCommand.DrawCommandType.RoundedRect:
                    bdc = new DrawRoundedRect();
                    break;
                case BaseDrawCommand.DrawCommandType.Perlin:
                    bdc = new DrawPerlin();
                    break;
            }
            DrawCommands.Insert(_index + 1, bdc);

        }

        /// <summary>
        /// Removes a draw command at the provided index.
        /// </summary>
        /// <param name="_i">Index.</param>
        private void RemoveDrawCommand(int _index)
        {
            if (_index != -1)
            {
                DrawCommands.RemoveAt(_index);
            }
        }

        private void RearrangeDrawCommand(int _currentIndex, int _targetIndex)
        {
            if (_currentIndex != -1 && _currentIndex + _targetIndex >= 0 && _currentIndex + _targetIndex < DrawCommands.Count)
            {
                List<BaseDrawCommand> newList = new List<BaseDrawCommand>();

                for (int i = 0; i < DrawCommands.Count; i++)
                {
                    if (i == _currentIndex)
                    {
                        newList.Add(DrawCommands[_currentIndex + _targetIndex]);

                    }
                    else if (i == _currentIndex + _targetIndex)
                    {
                        newList.Add(DrawCommands[_currentIndex]);

                    }
                    else
                    {
                        newList.Add(DrawCommands[i]);
                    }
                }
                DrawCommands = newList;
            }
        }

        /// <summary>
        /// This renders the GUI for the selection tools
        /// </summary>
        /// <param name="_index">Index.</param>
        private void SelectionGUI(int _index)
        {
            Color t = GUI.color;
            GUI.color = Color.cyan;
            if (GUILayout.Button("^"))
            {
                CommandToRearrange = _index;
                RearrangeAmount = -1;
            }
            if (GUILayout.Button("V"))
            {
                CommandToRearrange = _index;
                RearrangeAmount = 1;
            }
            GUI.color = t;

        }

        /// <summary>
        /// Renders the GUI for the Insertion tool
        /// </summary>
        /// <param name="_index">Index.</param>
        private void InsertionGUI(int _index)
        {
            if (GUILayout.Button("Insert New Draw Command"))
            {
                InsertDrawCommand(_index);
            }
            DrawCommandToInsert = (BaseDrawCommand.DrawCommandType)EditorGUILayout.EnumPopup(DrawCommandToInsert);
        }


        /// <summary>
        /// Renders the GUI for the Data section
        /// </summary>
        /// <param name="_index">Index.</param>
        private void DataGUI(int _index)
        {
            GUILayout.Label(_index.ToString() + " - " + DrawCommands[_index].Name);
            GUI.color = Color.yellow;
            if (GUILayout.Button(DrawCommands[_index].Hidden ? "+" : "-", GUILayout.MaxWidth(20)))
            {
                DrawCommands[_index].Hidden = !DrawCommands[_index].Hidden;
            }

            GUI.color = DrawCommands[_index].Visible ? Color.green : Color.red;
            if (GUILayout.Button("V", GUILayout.MaxWidth(20)))
            { 
                DrawCommands[_index].Visible = !DrawCommands[_index].Visible;
            }

            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
            {
                CommandToRemove = _index;
            }
            GUI.color = Color.white;
        }




    }
}
