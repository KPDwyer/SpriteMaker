using UnityEngine;
using UnityEditor;

namespace SpriteMaker
{
    [System.Serializable]
    /// <summary>
    /// Base Draw Command.  Subclass this to create custom draw commands
    /// </summary>
    public class BaseDrawCommand
    {

        /// <summary>
        /// Types of Draw Commands.  Need to update when creating a new Draw Command
        /// </summary>

        public enum DrawCommandType
        {
            Fill = 0,
            Circle = 1,
            Rect = 2,
            RoundedRect = 3,
            Perlin = 4,
            Voronoi = 5,
            RidgedMultifractal = 6,
            Billow = 7
        }


        /// <summary>
        /// Blendmode Enums.  Updatewhen adding a blend mode.
        /// </summary>
        public enum BlendMode
        {
            Opacity = 0,
            Additive = 1,
            Subtract = 2,
            Multiply = 3,
            Replace = 4
        }

        public BlendMode blendMode;
        public DrawCommandType myType;
        public string Name;
        public bool Visible = true;
        public bool Hidden = false;

        private Color cachedColor;

        /// <summary>
        /// when we serialize a draw command, it stores its data here so it gets serialized properly.
        /// Unity doesn't nattively support serializing derived classes, so this class has to hold the data from derived classes
        /// derived Draw Commands are responsible for storing and retrieving serialized data
        /// </summary>
        [System.Serializable]
        public struct SerializedData
        {
            public float[] serializedFloats;
            public Color[] serializedColors;
            [SerializeField]
            public Gradient[] serializedGradients;
        };

        public SerializedData data;

        /// <summary>
        /// Override this function to implement the drawing fucntion of a custom Draw Command
        /// </summary>
        /// <returns>The Canvas after the Draw Command has executed</returns>
        /// <param name="_input">The Canvas before the Draw COmmand has executed</param>
        /// <param name="_width">Width of image in pixels</param>
        /// <param name="_height">Height of image in pixels</param>
        public virtual Color[] DrawToColorArray(Color[] _input, int _width, int _height)
        {
            //some draw code here
            //for when we add blend modes

            return _input;

        }

        /// <summary>
        /// override this function to draw custom inputs for your draw commands.
        /// </summary>
        public virtual void DrawControls()
        {
            blendMode = (BaseDrawCommand.BlendMode)EditorGUILayout.EnumPopup(blendMode);

        }

        /// <summary>
        /// Helper function to appropriately blend a new pixel with the canvas' pixel.
        /// Please use this rather than editing the array directly to support BlendModes
        /// </summary>
        /// <returns>The blended color value.</returns>
        /// <param name="_pixel">Pixel to be blended</param>
        /// <param name="_canvas">Canvas Pixel to blend too</param>
        protected Color BlendPixelToCanvas(Color _pixel, Color _canvas)
        {
            cachedColor = _canvas;
            switch (blendMode)
            {
                case BlendMode.Opacity:

                    cachedColor.a = _pixel.a + _canvas.a;

                    cachedColor.r = (_pixel.r * _pixel.a) + ((1 - _pixel.a) * _canvas.r);
                    cachedColor.g = (_pixel.g * _pixel.a) + ((1 - _pixel.a) * _canvas.g);
                    cachedColor.b = (_pixel.b * _pixel.a) + ((1 - _pixel.a) * _canvas.b);

                    break;
                case BlendMode.Additive:
				
                    cachedColor.a = _pixel.a + _canvas.a;

                    cachedColor.r = Mathf.Min((_pixel.r * _pixel.a) + _canvas.r, 1.0f);
                    cachedColor.g = Mathf.Min((_pixel.g * _pixel.a) + _canvas.g, 1.0f);
                    cachedColor.b = Mathf.Min((_pixel.b * _pixel.a) + _canvas.b, 1.0f);


                    break;

                case BlendMode.Subtract:
                    cachedColor.a = _pixel.a + _canvas.a;

                    cachedColor.r = Mathf.Max(_canvas.r - (_pixel.r * _pixel.a), 0.0f);
                    cachedColor.g = Mathf.Max(_canvas.g - (_pixel.g * _pixel.a), 0.0f);
                    cachedColor.b = Mathf.Max(_canvas.b - (_pixel.b * _pixel.a), 0.0f);

                    break;

                case BlendMode.Multiply:
                    cachedColor.a = Mathf.Min(_pixel.a + _canvas.a, 1.0f);

                    cachedColor.r = Mathf.Lerp(_canvas.r, _canvas.r * (_pixel.r), _pixel.a);
                    cachedColor.g = Mathf.Lerp(_canvas.g, _canvas.g * (_pixel.g), _pixel.a);
                    cachedColor.b = Mathf.Lerp(_canvas.b, _canvas.b * (_pixel.b), _pixel.a);

                    break;
                case BlendMode.Replace:
                    
                    cachedColor.a = _pixel.a;

                    cachedColor.r = _pixel.r;
                    cachedColor.g = _pixel.g;
                    cachedColor.b = _pixel.b;

                    break;
            }

            return cachedColor;
        }

        /// <summary>
        /// Called before serializing this draw call.  Derived classes must extend this and populate SerializaedData to be correctly serialized;
        /// </summary>
        public virtual void OnBeforeSerialize()
        {
        }

        /// <summary>
        /// Called after serializing or anytime a BaseDrawCommand must be cast to a derived type.  builds derivedclass from data in SerializedData
        /// use this to populate properties from serializeddata
        /// </summary>
        /// <param name="bd">Bd.</param>
        public virtual void PopulateFromBase(BaseDrawCommand bd)
        {
         
            blendMode = bd.blendMode;
        }
            
    }
}
