using UnityEngine;
using UnityEditor;
namespace SpriteMaker{
	
	public class SpriteMakerEditor : EditorWindow {

		TextureBuilder texBuilder;
		DrawCommandManager drawCommandManager;

		SpritePreviewEditor spritePreview;

		private Vector2 ScrollPosition;


		private bool previewFocus = false;
		private bool updateOnChange = false;

		[MenuItem ("Window/SpriteMaker")]
		static void Init () {
			SpriteMakerEditor window = (SpriteMakerEditor)EditorWindow.GetWindow (typeof (SpriteMakerEditor),false,"Sprite Maker");
			window.texBuilder = new TextureBuilder ();
			window.drawCommandManager = new DrawCommandManager ();
			window.Show();
		}

		void OnGUI () {

			//if the editor window was left open on a new Unity bootup, init doesn't get called.
			if (texBuilder == null) {
				texBuilder = new TextureBuilder ();
			}
			if(drawCommandManager == null){
				drawCommandManager = new DrawCommandManager();
			}

			//we draw sprite info first so users can always hit preview
			GUISpriteInfo ();



			//scroll area for the draw commands
			ScrollPosition = EditorGUILayout.BeginScrollView (ScrollPosition);
			{
				EditorGUILayout.BeginVertical ();
				{
					GUIDrawCommands ();
				}
				EditorGUILayout.EndVertical ();	
			}
			EditorGUILayout.EndScrollView ();

			if (previewFocus) {
				spritePreview.Focus ();
				previewFocus = false;
			}



		}

		/// <summary>
		/// Shows the preview Texture.
		/// </summary>
		/// <param name="_tex">The texture to preview</param>
		private void ShowPreview(Texture2D _tex)
		{
			if (spritePreview == null) {
				spritePreview = (SpritePreviewEditor)EditorWindow.GetWindow (typeof(SpritePreviewEditor),false,"Sprite Preview");
				spritePreview.Show ();
			}
			spritePreview.SetTexture (_tex);
		}

		/// <summary>
		/// Previews the texture.
		/// </summary>
		private void PreviewTexture()
		{
			ShowPreview (texBuilder.BuildTexture (drawCommandManager.GetDrawCommands()));
		}


		/// <summary>
		/// Triggers a preview than asks the TextureBuilder to save the texture
		/// </summary>
		private void SaveTexture()
		{
			PreviewTexture ();
			texBuilder.SaveTexture ();
		}


		/// <summary>
		/// Draws info at the highest level for the sprite (Filename, pixel size, etc)
		/// </summary>
		private void GUISpriteInfo()
		{
			GUILayout.Label ("Sprite Info", EditorStyles.boldLabel);

			EditorGUILayout.BeginVertical ("Box");{

				texBuilder.fileName = EditorGUILayout.TextField ("File Name", texBuilder.fileName);
				EditorGUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Width");
					texBuilder.pixelWidth = GetIntFromTextField (EditorGUILayout.TextField (texBuilder.pixelWidth.ToString ()));		
					GUILayout.Label ("Height");

					texBuilder.pixelHeight = GetIntFromTextField (EditorGUILayout.TextField (texBuilder.pixelHeight.ToString ()));
				}
				EditorGUILayout.EndHorizontal ();

				updateOnChange = GUILayout.Toggle (updateOnChange, "Auto-Preview (Use Low Image Sizes)");

				EditorGUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button ("Preview Texture")) {
						PreviewTexture ();
						previewFocus = true;
					}
					if (updateOnChange) {
						PreviewTexture ();
					}
					if (GUILayout.Button ("Save Texture")) {
						SaveTexture ();
						previewFocus = true;
					}
				}EditorGUILayout.EndHorizontal ();

			}EditorGUILayout.EndVertical ();
		}

		/// <summary>
		/// sets up the GUI for drawCommandManager to draw the commands
		/// </summary>
		private void GUIDrawCommands()
		{

			GUILayout.Label ("Draw Commands", EditorStyles.boldLabel);

			EditorGUILayout.BeginVertical ("Box");{


				drawCommandManager.DrawControls ();



			}EditorGUILayout.EndVertical ();
		}


		int temp = 0;

		/// <summary>
		/// Helper function for textfields using Ints
		/// </summary>
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
}