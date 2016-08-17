using UnityEngine;
using UnityEditor;


public class BaseDrawCommand {

	public virtual Color32[] DrawToColorArray(Color32[] _input, int _width, int _height)
	{
		//some draw code here


		return _input;

	}

	public virtual void DrawControls()
	{
		
	}



}
