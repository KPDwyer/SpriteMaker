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

		//d

		for (int x = leftBounds; x <= rightBounds; x++) {
			tempVector.x = x;
			if (x >= 0 && x < _width) {
				for (int y = lowerBounds; y <= upperBounds; y++) {
					if (y >= 0 && y < _height) {
						tempVector.y = y;
						Color c = col;
						c.a = 1.0f - Vector2.Distance (positionVector, tempVector) / pixelRadius;
						if (c.a > aliasEdge) {
							c.a = 1.0f;
						} else {
							c.a = Mathf.InverseLerp (0, aliasEdge, c.a); 
						}
						c.r *= c.a;
						c.g *= c.a;
						c.b *= c.a;


						_input [y * _width + x] += c;

					}
				}
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
