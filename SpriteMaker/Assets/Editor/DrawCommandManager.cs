using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DrawCommandManager {

	public List<BaseDrawCommand> DrawCommands = new List<BaseDrawCommand>();

	public DrawCommandManager()
	{
		DrawCommands.Add(new DrawFill());
	}

	public void DrawControls()
	{

		for (int i = 0; i < DrawCommands.Count; i++) {
			DrawCommands [i].DrawControls ();
		}
	}

	public List<BaseDrawCommand> GetDrawCommands()
	{
		return DrawCommands;
	}

}
