using UnityEngine;
using UnityEditor;
using System.Collections;
namespace SpriteMaker{

	/// <summary>
	/// Draw Command to fill the entire canvas with a color
	/// </summary>
	public class DrawFill : BaseDrawCommand {

		/// <summary>
		/// Color to fill canvas with
		/// </summary>
		public Color fill;

		public override Color[] DrawToColorArray (Color[] _input, int _width, int _height)
		{
			for (int x = 0; x < _input.Length; x++) {
				_input[x] = BlendPixelToCanvas(fill,_input[x]);
			}

			return base.DrawToColorArray (_input, _width, _height);
		}

		public override void DrawControls ()
		{
			fill = EditorGUILayout.ColorField ("Fill Color", fill);
			base.DrawControls ();
		}

	}
}
