using UnityEngine;
using UnityEditor;
public class SpriteMakerEditor : EditorWindow {
	string myString = "Hello World";
	TextureBuilder texBuilder;


	[MenuItem ("Window/SpriteMaker")]
	static void Init () {
		SpriteMakerEditor window = (SpriteMakerEditor)EditorWindow.GetWindow (typeof (SpriteMakerEditor));
		window.Show();
		window.texBuilder = new TextureBuilder ();


	}



	void OnGUI () {
		GUILayout.Label ("Temp Debug", EditorStyles.boldLabel);

		if (texBuilder == null)
			texBuilder = new TextureBuilder ();
		
		texBuilder.fileName = EditorGUILayout.TextField ("File Name", texBuilder.fileName);
		texBuilder.pixelWidth = GetIntFromTextField(EditorGUILayout.TextField("Width", texBuilder.pixelWidth.ToString()));
		texBuilder.pixelHeight = GetIntFromTextField(EditorGUILayout.TextField("Height", texBuilder.pixelHeight.ToString()));

		//texBuilder.pixelWidth = EditorGUILayout.TextField("Width",texBuilder.fileName,


		if (GUILayout.Button ("Make Texture")) {
			MakeTexture ();
		}
	}

	private void MakeTexture()
	{
		texBuilder.BuildTexture ();
	}

	int temp = 0;
	private int GetIntFromTextField(string _text)
	{
		temp = 0;
		if (int.TryParse (_text, out temp)) {
			temp = Mathf.Abs (temp); 	//No negatives
		} else {
			temp = 0;
		}
		return temp;
	}


}