using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SpriteMaker
{



    /// <summary>
    /// Draws a rounded Rectangle onto the canvas
    /// </summary>
    public class DrawRoundedRect : BaseDrawCommand
    {


        public float CenterX = 0.5f, CenterY = 0.5f;
        public float Width = 0.8f, Height = 0.8f;
        public float CornerRadius = 0.1f;
        public float Smoothness = 0.0f;
        public Color rectColor = Color.red;

        private int pixelPosX;
        private int pixelPosY;
        private int pixelWidth;
        private int pixelHeight;
        private int pixelRadius;

        private Vector2 positionVector;
        private Vector2 tempVector;

        //TODO: CornerRadius affects the size of the Rectangle for some reason.
        //TODO: Corner Radius can be larger than total rectangle size.
        //TODO: general cleanup.
        //TODO: the way the radius is defined and used here leaves much to be desired for extreme roundedrects
        //TODO: Lazy Implementation - there's no need to loop through the array so much, can be condensed.
        public override Color[] DrawToColorArray(Color[] _input, int _width, int _height)
        {

            Color[] tempArray = new Color[_input.Length];

            pixelPosX = Mathf.CeilToInt(CenterX * (float)_width);
            pixelPosY = Mathf.CeilToInt(CenterY * (float)_height);
            pixelWidth = Mathf.CeilToInt(Width * (_width));
            pixelHeight = Mathf.CeilToInt(Height * (_height));
            pixelRadius = Mathf.CeilToInt(CornerRadius * ((_width + _height) / 2));

            //corners
            tempArray = DrawCircle(tempArray, _width, _height, pixelPosX + (pixelWidth / 2) - (pixelRadius / 2), pixelPosY + (pixelHeight / 2) - (pixelRadius / 2));
            tempArray = DrawCircle(tempArray, _width, _height, pixelPosX - (pixelWidth / 2) + (pixelRadius / 2), pixelPosY + (pixelHeight / 2) - (pixelRadius / 2));
            tempArray = DrawCircle(tempArray, _width, _height, pixelPosX + (pixelWidth / 2) - (pixelRadius / 2), pixelPosY - (pixelHeight / 2) + (pixelRadius / 2));
            tempArray = DrawCircle(tempArray, _width, _height, pixelPosX - (pixelWidth / 2) + (pixelRadius / 2), pixelPosY - (pixelHeight / 2) + (pixelRadius / 2));

            //body
            tempArray = DrawRect(tempArray, 
                _width, 
                _height,
                pixelPosX - (pixelWidth / 2) + (pixelRadius / 2),
                pixelPosX + (pixelWidth / 2) - (pixelRadius / 2), 
                pixelPosY + (pixelHeight / 2) + (pixelRadius / 2),
                pixelPosY - (pixelHeight / 2) - (pixelRadius / 2)
            );
            tempArray = DrawRect(tempArray, 
                _width, 
                _height,
                pixelPosX - (pixelWidth / 2) - (pixelRadius / 2),
                pixelPosX + (pixelWidth / 2) + (pixelRadius / 2), 
                pixelPosY + (pixelHeight / 2) - (pixelRadius / 2),
                pixelPosY - (pixelHeight / 2) + (pixelRadius / 2)
            );
				


            for (int x = 0; x < tempArray.Length; x++)
            {

                Color c = tempArray[x];
                _input[x] = BlendPixelToCanvas(c, _input[x]);

            }

            return base.DrawToColorArray(_input, _width, _height);
        }

        private Color[] DrawCircle(Color[] _input, int _width, int _height, int _pixPosX, int _pixPosY)
        {

            positionVector.x = _pixPosX;
            positionVector.y = _pixPosY;

            int leftBounds = _pixPosX - pixelRadius;
            int rightBounds = _pixPosX + pixelRadius;
            int upperBounds = _pixPosY + pixelRadius;
            int lowerBounds = _pixPosY - pixelRadius;

            for (int x = leftBounds; x <= rightBounds; x++)
            {
                tempVector.x = x;
                if (x >= 0 && x < _width)
                {
                    for (int y = lowerBounds; y <= upperBounds; y++)
                    {
                        if (y >= 0 && y < _height)
                        {
                            tempVector.y = y;
                            Color c = rectColor;
                            c.a = 1.0f - Vector2.Distance(positionVector, tempVector) / pixelRadius;
                            if (c.a > Smoothness)
                            {
                                c.a = 1.0f;
                            }
                            else
                            {
                                c.a = Mathf.InverseLerp(0, Smoothness, c.a); 
                            }
                            c.a *= rectColor.a;


                            if (c.a != 0)
                            {
                                c.r = (c.r * c.a) + ((1 - c.a) * _input[y * _width + x].r);
                                c.g = (c.g * c.a) + ((1 - c.a) * _input[y * _width + x].g);
                                c.b = (c.b * c.a) + ((1 - c.a) * _input[y * _width + x].b);

                                c.a = Mathf.Max(c.a, _input[y * _width + x].a);

                                _input[y * _width + x] = c;
                            }


                        }
                    }
                }
            }

            return _input;
        }

        private Color[] DrawRect(Color[] _input, int _width, int _height, int leftBounds, int rightBounds, int upperBounds, int lowerBounds)
        {
            for (int x = leftBounds; x <= rightBounds; x++)
            {
                if (x >= 0 && x < _width)
                {
                    for (int y = lowerBounds; y <= upperBounds; y++)
                    {
                        if (y >= 0 && y < _height)
                        {


                            Color c = rectColor;


                            c.r = (c.r * c.a) + ((1 - c.a) * _input[y * _width + x].r);
                            c.g = (c.g * c.a) + ((1 - c.a) * _input[y * _width + x].g);
                            c.b = (c.b * c.a) + ((1 - c.a) * _input[y * _width + x].b);

                            c.a = Mathf.Max(c.a, _input[y * _width + x].a);

                            _input[y * _width + x] = c;

                        }
                    }
                }
            }
            return _input;
        }

        public override void DrawControls()
        {
            Name = "RoundedRect";
            rectColor = EditorGUILayout.ColorField("Color", rectColor);
            CenterX = float.Parse(EditorGUILayout.TextField("X Position", CenterX.ToString()));
            CenterY = float.Parse(EditorGUILayout.TextField("Y Position", CenterY.ToString()));
            Width = float.Parse(EditorGUILayout.TextField("Width", Width.ToString()));
            Height = float.Parse(EditorGUILayout.TextField("Height", Height.ToString()));
            CornerRadius = float.Parse(EditorGUILayout.TextField("Corner Radius", CornerRadius.ToString()));
            Smoothness = float.Parse(EditorGUILayout.TextField("Smoothness", Smoothness.ToString()));


            base.DrawControls();
        }


        #region SERIALIZATION

        public override void OnBeforeSerialize()
        {
            data = new SerializedData();

            data.serializedFloats = new float[6];
            data.serializedFloats[0] = CenterX;
            data.serializedFloats[1] = CenterY;
            data.serializedFloats[2] = Width;
            data.serializedFloats[3] = Height;
            data.serializedFloats[4] = CornerRadius;
            data.serializedFloats[5] = Smoothness;

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
            CornerRadius = bd.data.serializedFloats[4];
            Smoothness = bd.data.serializedFloats[5];

            rectColor = bd.data.serializedColors[0];

            base.PopulateFromBase(bd);
        }

        #endregion

    }
}
