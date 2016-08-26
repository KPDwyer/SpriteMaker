using UnityEngine;
using UnityEditor;
namespace SpriteMaker{

	/// <summary>
	/// Base Draw Command.  Subclass this to create custom draw commands
	/// </summary>
	public class BaseDrawCommand {

		/// <summary>
		/// Types of Draw Commands.  Need to update when creating a new Draw Command
		/// </summary>
		public enum DrawCommandType {
			Fill = 0,
			Circle = 1,
			Rect = 2,
			RoundedRect = 3
		}
				
		private Color cachedColor;

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
			
		}

		/// <summary>
		/// Helper function to appropriately blend a new pixel with the canvas' pixel.
		/// Please use this rather than editing the array directly to (eventually) support BlendModes
		/// </summary>
		/// <returns>The blended color value.</returns>
		/// <param name="_pixel">Pixel to be blended</param>
		/// <param name="_canvas">Canvas Pixel to blend too</param>
		protected Color BlendPixelToCanvas(Color _pixel, Color _canvas)
		{
			cachedColor = _canvas;
			cachedColor.r = (_pixel.r * _pixel.a) + ((1 - _pixel.a) * _canvas.r);
			cachedColor.g = (_pixel.g * _pixel.a) + ((1 - _pixel.a) * _canvas.g);
			cachedColor.b = (_pixel.b * _pixel.a) + ((1 - _pixel.a) * _canvas.b);

			cachedColor.a = _pixel.a + _canvas.a;

			return cachedColor;
		}



	}
}
