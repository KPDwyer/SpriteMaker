using UnityEngine;
using UnityEditor;
namespace SpriteMaker{

	/// <summary>
	/// Handles the SPrite Preview Window
	/// </summary>
	public class SpritePreviewEditor : EditorWindow {

		public Texture2D mTex;
		private Texture2D mGray;


		void OnGUI()
		{
			if (mTex != null) {
				EditorGUI.DrawPreviewTexture (new Rect(0,0,position.width,position.height),mTex,null,ScaleMode.ScaleToFit,1.0f);
			}
		}

		public void SetTexture(Texture2D _tex)
		{

			mTex = _tex;
			mTex.filterMode = FilterMode.Point;
			base.Repaint ();

		}
	}
}
