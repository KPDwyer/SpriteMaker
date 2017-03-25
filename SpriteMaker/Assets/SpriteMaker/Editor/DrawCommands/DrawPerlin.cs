using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpriteMaker
{

    public class DrawPerlin : BaseDrawCommand
    {
        public float HorizontalScale = 6.4f;
        public float VerticalScale = 6.4f;
        public float xOffset;
        public float yOffset;
        public Gradient gradient = new Gradient();

        private ScriptableGradientAsset gradientObject;

        private float xPosition;
        private float yPosition;


        public override Color[] DrawToColorArray(Color[] _input, int _width, int _height)
        {
            
            for (int y = 0; y < _height; y++)
            {
                yPosition = (float)y / (float)_height;
                for (int x = 0; x < _width; x++)
                {
                    xPosition = (float)x / (float)_width;

                    _input[y * _width + x] = BlendPixelToCanvas(
                        gradient.Evaluate(
                            Mathf.PerlinNoise(
                                (xPosition * HorizontalScale) + xOffset, 
                                (yPosition * VerticalScale) + yOffset
                            ))
                        , _input[y * _width + x]);
                }
            }


            return base.DrawToColorArray(_input, _width, _height);
        }

        public override void DrawControls()
        {
            Name = "Perlin Noise";
            HorizontalScale = float.Parse(EditorGUILayout.TextField("Horizontal Scale", HorizontalScale.ToString()));
            VerticalScale = float.Parse(EditorGUILayout.TextField("Vertical Scale", VerticalScale.ToString()));

            //TODO: I can't believe this is how this must be done
            //To get a nice gradient picker, we need to alter the serializedproperty of a serializedobject
            //since this class isn;t serializable (derives fro something other than monobehaviour or scriptableobject),
            //we create a SerializedObject that just has a gradient on it, set its gradient to out gradient, and show the field.
            //if its changed, we apply those settings back to the serialized object and set our gradient from it.
            if (gradientObject == null)
            {
                gradientObject = ScriptableObject.CreateInstance<ScriptableGradientAsset>();
                gradientObject.gradient = gradient;
            }

            EditorGUI.BeginChangeCheck();
            {
                SerializedObject obj = new SerializedObject(gradientObject);
                SerializedProperty sgrad = obj.FindProperty("gradient");
                EditorGUILayout.PropertyField(sgrad, true, null);
                if (EditorGUI.EndChangeCheck())
                {
                    obj.ApplyModifiedProperties();
                    gradient = gradientObject.gradient;
                }


            }



            if (GUILayout.Button("Shuffle"))
            {
                xOffset = Random.Range(-9999, 9999);
                yOffset = Random.Range(-9999, 9999);
            }
            base.DrawControls();
        }

        #region SERIALIZATION

        public override void OnBeforeSerialize()
        {
            data = new SerializedData();

            data.serializedFloats = new float[4];
            data.serializedFloats[0] = HorizontalScale;
            data.serializedFloats[1] = VerticalScale;
            data.serializedFloats[2] = xOffset;
            data.serializedFloats[3] = yOffset;

            data.serializedGradients = new Gradient[1];
            data.serializedGradients[0] = gradient;

            base.OnBeforeSerialize();
        }

        public override void PopulateFromBase(BaseDrawCommand bd)
        {
            HorizontalScale = bd.data.serializedFloats[0];
            VerticalScale = bd.data.serializedFloats[1];
            xOffset = bd.data.serializedFloats[2];
            yOffset = bd.data.serializedFloats[3];


            gradient = bd.data.serializedGradients[0];




            base.PopulateFromBase(bd);
        }

        #endregion
	
    }
}
