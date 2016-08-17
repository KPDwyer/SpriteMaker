using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class TextureBuilder {

	public string fileName = "Asset";
	public int pixelWidth = 64;
	public int pixelHeight = 64;

	public TextureBuilder()
	{
	}



	public void BuildTexture()
	{

		Texture2D tex = new Texture2D(pixelWidth, pixelHeight, TextureFormat.ARGB32, false);

		DrawCircle dc = new DrawCircle ();
		tex = dc.DrawToTexture2D (tex);

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Object.DestroyImmediate(tex);


		CheckVars ();
		//For testing purposes, also write to a file in the project folder
		File.WriteAllBytes(Application.dataPath + "/"+fileName+".png", bytes);


		AssetDatabase.Refresh ();

	}

	private void CheckVars()
	{
		if (pixelHeight == pixelWidth && pixelHeight == 0) {
			pixelWidth = pixelHeight = 64; //default to 64x64
		}

		if (string.IsNullOrEmpty (fileName)) {
			
		}
	}
}
