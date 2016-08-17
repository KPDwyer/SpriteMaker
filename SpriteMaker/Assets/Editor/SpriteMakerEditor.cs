using UnityEngine;
using UnityEditor;
public class SpriteMakerEditor : EditorWindow {

	TextureBuilder texBuilder;
	DrawCommandManager drawCommands;

	SpritePreviewEditor spritePreview;

	[MenuItem ("Window/SpriteMaker")]
	static void Init () {
		SpriteMakerEditor window = (SpriteMakerEditor)EditorWindow.GetWindow (typeof (SpriteMakerEditor));
		window.Show();
	}

	void OnGUI () {
		GUILayout.Label ("Sprite Info", EditorStyles.boldLabel);

		if (texBuilder == null)
			texBuilder = new TextureBuilder ();

		if (drawCommands == null)
			drawCommands = new DrawCommandManager ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ("Box");

		texBuilder.fileName = EditorGUILayout.TextField ("File Name", texBuilder.fileName);
		texBuilder.pixelWidth = GetIntFromTextField(EditorGUILayout.TextField("Width", texBuilder.pixelWidth.ToString()));
		texBuilder.pixelHeight = GetIntFromTextField(EditorGUILayout.TextField("Height", texBuilder.pixelHeight.ToString()));



		if (GUILayout.Button ("Make Texture")) {
			MakeTexture ();
		}


		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ();
		GUILayout.Label ("Draw Commands", EditorStyles.boldLabel);

		drawCommands.DrawControls ();
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();


		EditorGUILayout.EndHorizontal ();
		 



	}

	private void ShowPreview(Texture2D _tex)
	{
		if (spritePreview == null) {
			spritePreview = (SpritePreviewEditor)EditorWindow.GetWindow (typeof(SpritePreviewEditor));
			spritePreview.Show ();
		}
		spritePreview.Focus ();
		spritePreview.SetTexture (_tex);
	}

	private void MakeTexture()
	{
		ShowPreview (texBuilder.BuildTexture (drawCommands.GetDrawCommands()));
		//texBuilder.SaveTexture ();
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