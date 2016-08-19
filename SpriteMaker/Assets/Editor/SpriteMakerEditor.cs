using UnityEngine;
using UnityEditor;
public class SpriteMakerEditor : EditorWindow {

	TextureBuilder texBuilder;
	DrawCommandManager drawCommands;

	SpritePreviewEditor spritePreview;

	private bool previewFocus = false;

	[MenuItem ("Window/SpriteMaker")]
	static void Init () {
		SpriteMakerEditor window = (SpriteMakerEditor)EditorWindow.GetWindow (typeof (SpriteMakerEditor));
		window.Show();
	}

	void OnGUI () {

		if (texBuilder == null)
			texBuilder = new TextureBuilder ();

		if (drawCommands == null)
			drawCommands = new DrawCommandManager ();

		EditorGUILayout.BeginVertical ();{

			GUISpriteInfo ();
			GUIDrawCommands ();


		}EditorGUILayout.EndVertical ();

		if (previewFocus) {
			spritePreview.Focus ();
			previewFocus = false;
		}



	}

	private void ShowPreview(Texture2D _tex)
	{
		if (spritePreview == null) {
			spritePreview = (SpritePreviewEditor)EditorWindow.GetWindow (typeof(SpritePreviewEditor));
			spritePreview.Show ();
		}
		spritePreview.SetTexture (_tex);
	}

	private void MakeTexture()
	{
		ShowPreview (texBuilder.BuildTexture (drawCommands.GetDrawCommands()));
		//texBuilder.SaveTexture ();
	}

	private void GUISpriteInfo()
	{
		GUILayout.Label ("Sprite Info", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical ("Box");{

			texBuilder.fileName = EditorGUILayout.TextField ("File Name", texBuilder.fileName);
			EditorGUILayout.BeginHorizontal (); {
				GUILayout.Label ("Width");
				texBuilder.pixelWidth = GetIntFromTextField (EditorGUILayout.TextField ( texBuilder.pixelWidth.ToString ()));		
				GUILayout.Label ("Height");

				texBuilder.pixelHeight = GetIntFromTextField (EditorGUILayout.TextField (texBuilder.pixelHeight.ToString ()));
			}EditorGUILayout.EndHorizontal();



			if (GUILayout.Button ("Make Texture")) {
				MakeTexture ();
				previewFocus = true;
			}

		}EditorGUILayout.EndVertical ();
	}


	private void GUIDrawCommands()
	{

		GUILayout.Label ("Draw Commands", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical ("Box");{


			drawCommands.DrawControls ();



		}EditorGUILayout.EndVertical ();
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