using UnityEngine;
using UnityEditor;
using System.Collections;

public class DrawFill : BaseDrawCommand {

	public Color32 fill;

	public override Color32[] DrawToColorArray (Color32[] _input, int _width, int _height)
	{
		for (int i = 0; i < _input.Length; i++) {
			_input[i] = fill;
		}

		return base.DrawToColorArray (_input, _width, _height);
	}

	public override void DrawControls ()
	{

		fill = EditorGUILayout.ColorField ("Fill Color", fill);
		base.DrawControls ();
	}

}
