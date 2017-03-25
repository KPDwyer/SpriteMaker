using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SpriteMaker
{


    /// <summary>
    /// Used to draw a Rect on the canvas.
    /// </summary>
    public class DrawRect : BaseDrawCommand
    {

        public float CenterX, CenterY;
        public float Width;
        public float Height;
        public Color rectColor;

        private int pixelPosX;
        private int pixelPosY;
        private int pixelWidth;
        private int pixelHeight;

        public override Color[] DrawToColorArray(Color[] _input, int _width, int _height)
        {

            pixelPosX = Mathf.CeilToInt(CenterX * (float)_width);
            pixelPosY = Mathf.CeilToInt(CenterY * (float)_height);
            pixelWidth = Mathf.CeilToInt(Width * (_width));
            pixelHeight = Mathf.CeilToInt(Height * (_height));




            int leftBounds = pixelPosX - (pixelWidth / 2);
            int rightBounds = pixelPosX + (pixelWidth / 2);
            int lowerBounds = pixelPosY - (pixelHeight / 2);
            int upperBounds = pixelPosY + (pixelHeight / 2);



            for (int x = leftBounds; x <= rightBounds; x++)
            {
                if (x >= 0 && x < _width)
                {
                    for (int y = lowerBounds; y <= upperBounds; y++)
                    {
                        if (y >= 0 && y < _height)
                        {

                            _input[y * _width + x] = BlendPixelToCanvas(rectColor, _input[y * _width + x]);

                        }
                    }
                }
            }


            return base.DrawToColorArray(_input, _width, _height);
        }

        public override void DrawControls()
        {
            Name = "Rectangle";
            rectColor = EditorGUILayout.ColorField("Color", rectColor);
            CenterX = float.Parse(EditorGUILayout.TextField("X Position", CenterX.ToString()));
            CenterY = float.Parse(EditorGUILayout.TextField("Y Position", CenterY.ToString()));
            Width = float.Parse(EditorGUILayout.TextField("Width", Width.ToString()));
            Height = float.Parse(EditorGUILayout.TextField("Height", Height.ToString()));


            base.DrawControls();
        }

        #region SERIALIZATION

        public override void OnBeforeSerialize()
        {
            data = new SerializedData();

            data.serializedFloats = new float[4];
            data.serializedFloats[0] = CenterX;
            data.serializedFloats[1] = CenterY;
            data.serializedFloats[2] = Width;
            data.serializedFloats[3] = Height;


            data.serializedColors = new Color[1];
            data.serializedColors[0] = rectColor;


            base.OnBeforeSerialize();
        }

        public override void PopulateFromBase(BaseDrawCommand bd)
        {
            CenterX = bd.data.serializedFloats[0];
            CenterY = bd.data.serializedFloats[1];
            Width = bd.data.serializedFloats[2];
            Height = bd.data.serializedFloats[3];


            rectColor = bd.data.serializedColors[0];


            base.PopulateFromBase(bd);
        }

        #endregion


    }
}
