using UnityEngine;
using System.Collections;

public class DrawCircle : DrawCommand {

	public Color col = Color.red;
	public int posX = 32;
	public int posY = 32;
	public int radius = 30;
	public float aliasEdge = 0.05f;

	private Vector2 positionVector;
	private Vector2 tempVector;

	public override Texture2D DrawToTexture2D (Texture2D _input)
	{
		positionVector.x = posX;
		positionVector.y = posY;
		Color32[] tempArray = _input.GetPixels32();

		int leftBounds = posX - radius;
		int rightBounds = posX + radius;
		int upperBounds = posY + radius;
		int lowerBounds = posY - radius;



		for (int x = leftBounds; x <= rightBounds; x++) {
			tempVector.x = x;
			for (int y = lowerBounds; y <= upperBounds; y++) {
				tempVector.y = y;

				col.a = 1.0f-Vector2.Distance (positionVector, tempVector)/radius;
				if (col.a > aliasEdge) {
					col.a = 1.0f;
				} else {
					col.a = Mathf.InverseLerp(0,aliasEdge,col.a); 
				}

				tempArray [y * _input.width + x] = col;


			}
		}


		_input.SetPixels32(tempArray);
		_input.Apply ();


		return base.DrawToTexture2D (_input);
	}
}
