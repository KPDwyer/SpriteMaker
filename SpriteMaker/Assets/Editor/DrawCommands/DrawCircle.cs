using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SpriteMaker{

	/// <summary>
	/// used to draw a circle on the canvas.
	/// </summary>
	public class DrawCircle : BaseDrawCommand {

		public Color color = Color.red;
		public float positionX = 0.5f;
		public float positionY = 0.5f;
		public float radiusFloat = 0.4f;
		public float smoothness = 0.05f;


		private int pixelPosX;
		private int pixelPosY;
		private int pixelRadius;

		private Vector2 positionVector;
		private Vector2 tempVector;


		//TODO: currently uses a distance check to determine a circle.  gives better results at lower res than initial circle. investigate.

		public override Color[] DrawToColorArray (Color[] _input, int _width, int _height)
		{
			
			//convert our relative values to absolute pixel values
			pixelPosX = Mathf.CeilToInt(positionX * (float)_width);
			pixelPosY = Mathf.CeilToInt(positionY * (float)_height);
			pixelRadius = Mathf.CeilToInt(radiusFloat * ((_width+_height)/2));

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
							Color c = color;

							c.a = 1.0f - Vector2.Distance (positionVector, tempVector) / pixelRadius;
							if (c.a > smoothness) {
								c.a = 1.0f;
							} else {
								c.a = Mathf.InverseLerp (0, smoothness, c.a); 
							}
							c.a *= color.a;


							_input [y * _width + x] = BlendPixelToCanvas (c, _input [y * _width + x]);
						}
					}
				}
			}
			return base.DrawToColorArray (_input,_width,_height);
		}


		public override void DrawControls ()
		{

			color = EditorGUILayout.ColorField ("Color", color);
			positionX = float.Parse(EditorGUILayout.TextField ("X Position", positionX.ToString()));
			positionY = float.Parse(EditorGUILayout.TextField ("Y Position", positionY.ToString()));
			radiusFloat = float.Parse(EditorGUILayout.TextField ("Radius", radiusFloat.ToString()));
			smoothness = float.Parse(EditorGUILayout.TextField ("Smoothness", smoothness.ToString()));


			base.DrawControls ();
		}
	}
}
