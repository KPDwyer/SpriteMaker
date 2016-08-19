using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DrawCommandManager {

	public List<BaseDrawCommand> DrawCommands = new List<BaseDrawCommand>();

	private BaseDrawCommand.DrawCommandType DrawCommandToInsert;

	public DrawCommandManager()
	{
		DrawCommands.Add(new DrawFill());
	}

	public void DrawControls()
	{
		EditorGUILayout.BeginHorizontal ();{

			InsertionGUI (-1);
		}EditorGUILayout.EndHorizontal ();

		for (int i = 0; i < DrawCommands.Count; i++) {
			EditorGUILayout.BeginHorizontal ("Box");{
				

				EditorGUILayout.BeginVertical ("Box");{
					DrawCommands [i].DrawControls ();
				}EditorGUILayout.EndVertical ();
				EditorGUILayout.BeginVertical (GUILayout.Width(25));{
					SelectionGUI (i);
				}EditorGUILayout.EndVertical ();


			}EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();{

				InsertionGUI (i);
			}EditorGUILayout.EndHorizontal ();


		}

	}

	public List<BaseDrawCommand> GetDrawCommands()
	{
		return DrawCommands;
	}

	public void InsertDrawCommand(int _index)
	{
		BaseDrawCommand bdc;
		switch (DrawCommandToInsert) {
		case BaseDrawCommand.DrawCommandType.Fill:
			bdc = new DrawFill ();
			break;
		case BaseDrawCommand.DrawCommandType.Circle:
		default:
			bdc = new DrawCircle ();
			break;
		}
		DrawCommands.Insert (_index + 1, bdc);

	}

	//TODO: this
	private void SelectionGUI(int _i)
	{
		Color t = GUI.color;
		GUI.color = Color.red;
		if (GUILayout.Button ("X")) {
		}
		GUI.color = Color.cyan;
		GUILayout.Button ("^");
		GUILayout.Button ("V");
		GUI.color = t;

	}
	private void InsertionGUI(int _i)
	{
		if (GUILayout.Button ("Insert New Draw Command")) {
			InsertDrawCommand (_i);
		}
		DrawCommandToInsert = (BaseDrawCommand.DrawCommandType)EditorGUILayout.EnumPopup (DrawCommandToInsert);
	}



}
