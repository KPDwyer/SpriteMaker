using UnityEngine;
using UnityEditor;


public class BaseDrawCommand {

	public enum DrawCommandType {
		Fill = 0,
		Circle = 1
	}
			

	public virtual Color32[] DrawToColorArray(Color32[] _input, int _width, int _height)
	{
		//some draw code here


		return _input;

	}

	public virtual void DrawControls()
	{
		
	}



}
