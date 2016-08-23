using UnityEngine;
using UnityEditor;

public class SpritePreviewEditor : EditorWindow {

	public Texture2D mTex;
	private Material mat;
	private Texture2D mGray;


	void OnGUI()
	{
		if (mTex != null) {
			
			EditorGUI.DrawPreviewTexture (new Rect(0,0,position.width,position.height),mTex,null,ScaleMode.ScaleToFit,1.0f);
		}
	}

	public void SetTexture(Texture2D _tex)
	{
		if (mat == null) {
			mat = new Material(Shader.Find("UI/Default"));
		}
		mTex = _tex;
		mTex.filterMode = FilterMode.Point;
		mTex.alphaIsTransparency = true;

	}
}
