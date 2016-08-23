using UnityEngine;
using UnityEditor;


public class BaseDrawCommand {

	public enum DrawCommandType {
		Fill = 0,
		Circle = 1
	}
			

	public virtual Color[] DrawToColorArray(Color[] _input, int _width, int _height)
	{
		//some draw code here
		//for when we add blend modes

		return _input;

	}

	public virtual void DrawControls()
	{
		
	}



}
