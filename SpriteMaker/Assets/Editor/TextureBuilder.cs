using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextureBuilder {

	public string fileName = "Asset";
	public int pixelWidth = 64;
	public int pixelHeight = 64;



	public TextureBuilder(){}

	private DrawCircle dc;
	private Texture2D tex;



	public Texture2D BuildTexture(List<BaseDrawCommand> _drawCommands)
	{

		tex = new Texture2D(pixelWidth, pixelHeight, TextureFormat.ARGB32, false);
		Color32[] colorArray = tex.GetPixels32 ();
		for (int i = 0; i < _drawCommands.Count; i++) {
			colorArray = _drawCommands [i].DrawToColorArray (colorArray,tex.width, tex.height);
		}
		tex.SetPixels32 (colorArray);
		tex.Apply ();
		return tex;


	}

	public Texture2D GetTexture()
	{
		return tex;
	}

	public void SaveTexture()
	{

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();

		CheckVars ();
		//For testing purposes, also write to a file in the project folder
		File.WriteAllBytes(Application.dataPath + "/"+fileName+".png", bytes);


		AssetDatabase.Refresh ();
	}

	public void DrawControls()
	{
		if (dc == null) {
			dc = new DrawCircle ();
		}
		dc.DrawControls ();
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
