using UnityEngine;
using UnityEditor;
using System.Collections;

public class DrawCircle : BaseDrawCommand {

	public Color col = Color.red;
	public float positionX = 0.5f;
	public float positionY = 0.5f;
	public float radiusfloat = 0.4f;
	public float aliasEdge = 0.05f;


	private int pixelPosX;
	private int pixelPosY;
	private int pixelRadius;

	private Vector2 positionVector;
	private Vector2 tempVector;

	public override Color32[] DrawToColorArray (Color32[] _input, int _width, int _height)
	{
		
		//convert our relative values to absolute pixel values
		pixelPosX = Mathf.CeilToInt(positionX * (float)_width);
		pixelPosY = Mathf.CeilToInt(positionY * (float)_height);
		pixelRadius = Mathf.CeilToInt(radiusfloat * ((_width+_height)/2));




		positionVector.x = pixelPosX;
		positionVector.y = pixelPosY;

		int leftBounds =  pixelPosX - pixelRadius;
		int rightBounds = pixelPosX + pixelRadius;
		int upperBounds = pixelPosY + pixelRadius;
		int lowerBounds = pixelPosY - pixelRadius;



		for (int x = leftBounds; x <= rightBounds; x++) {
			tempVector.x = x;
			for (int y = lowerBounds; y <= upperBounds; y++) {
				tempVector.y = y;

				col.a = 1.0f-Vector2.Distance (positionVector, tempVector)/pixelRadius;
				if (col.a > aliasEdge) {
					col.a = 1.0f;
				} else {
					col.a = Mathf.InverseLerp(0,aliasEdge,col.a); 
				}

				_input [y * _width + x] = col;


			}
		}
			


		return base.DrawToColorArray (_input,_width,_height);
	}


	public override void DrawControls ()
	{

		col = EditorGUILayout.ColorField ("Color", col);
		positionX = float.Parse(EditorGUILayout.TextField ("X Position", positionX.ToString()));
		positionY = float.Parse(EditorGUILayout.TextField ("Y Position", positionY.ToString()));
		radiusfloat = float.Parse(EditorGUILayout.TextField ("Radius", radiusfloat.ToString()));


		base.DrawControls ();
	}
}
