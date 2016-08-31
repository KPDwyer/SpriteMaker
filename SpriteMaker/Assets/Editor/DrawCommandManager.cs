using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace SpriteMaker{


	public class DrawCommandManager {

		public List<BaseDrawCommand> DrawCommands = new List<BaseDrawCommand>();

		private BaseDrawCommand.DrawCommandType DrawCommandToInsert;

		private int CommandToRemove = -1;
		private int CommandToRearrange = -1;
		private int RearrangeAmount = 0;

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

			RemoveDrawCommand (CommandToRemove);
			RearrangeDrawCommand (CommandToRearrange, RearrangeAmount);
			CommandToRemove = -1;
			CommandToRearrange = -1;
			RearrangeAmount = 0;

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
				bdc = new DrawCircle ();
				break;
			case BaseDrawCommand.DrawCommandType.Rect:
			default:
				bdc = new DrawRect ();
				break;
			case BaseDrawCommand.DrawCommandType.RoundedRect:
				bdc = new DrawRoundedRect ();
				break;
			}
			DrawCommands.Insert (_index + 1, bdc);

		}

		private void RemoveDrawCommand(int _i)
		{
			if (_i != -1) {
				DrawCommands.RemoveAt (_i);
			}
		}

		private void RearrangeDrawCommand(int _i, int target)
		{
			if (_i != -1 && _i+target >=0 && _i+target <DrawCommands.Count) {
				List<BaseDrawCommand> newList = new List<BaseDrawCommand> ();

				for (int i = 0; i < DrawCommands.Count; i++) {
					if (i == _i) {
						newList.Add (DrawCommands [_i + target]);

					} else if (i == _i + target) {
						newList.Add (DrawCommands [_i]);

					} else {
						newList.Add (DrawCommands [i]);
					}
				}
				DrawCommands = newList;
			}
		}

		//TODO: this
		private void SelectionGUI(int _i)
		{
			Color t = GUI.color;
			GUI.color = Color.red;
			if (GUILayout.Button ("X")) {
				CommandToRemove = _i;
			}
			GUI.color = Color.cyan;
			if (GUILayout.Button ("^")) {
				CommandToRearrange = _i;
				RearrangeAmount = -1;
			}
			if (GUILayout.Button ("V")) {
				CommandToRearrange = _i;
				RearrangeAmount = 1;
			}
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
}
