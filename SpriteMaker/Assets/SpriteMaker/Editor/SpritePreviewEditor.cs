using UnityEngine;
using UnityEditor;

namespace SpriteMaker
{

    /// <summary>
    /// Handles the SPrite Preview Window
    /// </summary>
    public class SpritePreviewEditor : EditorWindow
    {

        public Texture2D mTex;
        private Texture2D mGray;
        private Material eMaterial;
        private Material aMaterial;

        private const float scaleFactor = 16.0f;
        //probably best not to go past 24



        void OnGUI()
        {
            if (mGray == null)
            {
                mGray = Resources.Load<Texture2D>("Checkers");
            }

    
                    
            Rect g = new Rect(0, 0, position.width, position.height);
            EditorGUI.DrawPreviewTexture(g, mGray, null, ScaleMode.StretchToFill, 1.0f);


            if (eMaterial == null)
            {
                eMaterial = Resources.Load<Material>("EditorMaterial");
            }

            if (mTex != null)
            {
                EditorGUI.DrawPreviewTexture(new Rect(0, 0, position.width, position.height), mTex, eMaterial, ScaleMode.ScaleToFit, 1.0f);
            }
        }

        public void SetTexture(Texture2D _tex)
        {

            mTex = _tex;
            mTex.filterMode = FilterMode.Point;
            base.Repaint();

        }
    }
}
