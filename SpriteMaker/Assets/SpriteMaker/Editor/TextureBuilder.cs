using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SpriteMaker{

	public class TextureBuilder {

		// arbitrary default values
		public string fileName = "Asset";
		public int pixelWidth = 64;
		public int pixelHeight = 64;

		//Texture2D that we are manipulating
		private Texture2D tex;


		public TextureBuilder(){}


		/// <summary>
		/// Creates the Texture2D from the queue of Draw Commands
		/// </summary>
		/// <returns>The Texture2D.</returns>
		/// <param name="_drawCommands">Queue of Draw Commands used to generate texture</param>
		public Texture2D BuildTexture(List<BaseDrawCommand> _drawCommands)
		{

			tex = new Texture2D(pixelWidth, pixelHeight, TextureFormat.ARGB32, false);
			Color[] colorArray = tex.GetPixels ();

			//default seems to be a semitransparent gray, so we just dump those pixels right off the bat
			for (int i = 0; i < colorArray.Length; i++) {
				colorArray [i] = Color.clear;
			}

			for (int i = 0; i < _drawCommands.Count; i++) {
				if (_drawCommands [i].Visible) {
					colorArray = _drawCommands [i].DrawToColorArray (colorArray, tex.width, tex.height);
				}
			}
			tex.SetPixels (colorArray);
			tex.Apply ();
			return tex;


		}

		/// <summary>
		/// Gets the texture in its current state
		/// </summary>
		/// <returns>The texture.</returns>
		public Texture2D GetTexture()
		{
			return tex;
		}


		/// <summary>
		/// Saves the current Texture2D to a PNG
		/// </summary>
		public void SaveTexture()
		{

			// Encode texture into PNG
			byte[] bytes = tex.EncodeToPNG();

			CheckVars ();
			//For testing purposes, also write to a file in the project folder
			File.WriteAllBytes(Application.dataPath + "/"+fileName+".png", bytes);


			AssetDatabase.Refresh ();
		}
			

		/// <summary>
		/// Checks to make sure some inportant variables aren't null or empty
		/// </summary>
		private void CheckVars()
		{
			if (pixelHeight == pixelWidth && pixelHeight == 0) {
				pixelWidth = pixelHeight = 64; //default to 64x64
			}

			if (string.IsNullOrEmpty (fileName)) {
				fileName = "Asset";
				
			}
		}
	}
}
