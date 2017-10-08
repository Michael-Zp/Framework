using Zenseless.Geometry;
using OpenTK;
using OpenTK.Input;
using System;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Extension method class for OpenTK.INativeWindow
	/// </summary>
	public static class INativeWindowExtensions
	{
		/// <summary>
		/// Add Maya like camera handling. 
		/// </summary>
		/// <param name="window">window that receives input system events</param>
		/// <param name="camera">orbit camera events should be routed too.</param>
		public static void AddMayaCameraEvents(this INativeWindow window, CameraOrbit camera)
		{
			window.Resize += (s, e) => camera.Aspect = (float)window.Width / window.Height;
			window.MouseMove += (s, e) =>
			{
				if (ButtonState.Pressed == e.Mouse.LeftButton)
				{
					camera.Azimuth += 300 * e.XDelta / (float)window.Width;
					camera.Elevation += 300 * e.YDelta / (float)window.Height;
				}
			};
			window.MouseWheel += (s, e) =>
			{
				if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
				{
					camera.FovY *= (float)Math.Pow(1.05, e.DeltaPrecise);
				}
				else
				{
					camera.Distance *= (float)Math.Pow(1.05, e.DeltaPrecise);
				}
			};
		}

		/// <summary>
		/// Add key bindings; ESCAPE for closing; F11 for toggling fullscreen
		/// </summary>
		/// <param name="window">window that receives input system events</param>
		public static void AddDefaultExampleWindowEvents(this INativeWindow window)
		{
			window.KeyDown += (object sender, KeyboardKeyEventArgs e) =>
			{
				switch (e.Key)
				{
					case Key.Escape:
						window.Close();
						break;
					case Key.F11:
						window.WindowState = WindowState.Fullscreen == window.WindowState ? WindowState.Normal : WindowState.Fullscreen;
						break;
				}
			};
		}

		/// <summary>
		/// Converts pixel based coordinates to coordinates in range [0..1]²
		/// </summary>
		/// <param name="window">reference window</param>
		/// <param name="pixelX">pixel x-coordinate</param>
		/// <param name="pixelY">pixel y-coordinate</param>
		/// <returns>Coordinates in range [0..1]²</returns>
		public static System.Numerics.Vector2 ConvertCoords(this INativeWindow window, int pixelX, int pixelY)
		{
			return new System.Numerics.Vector2(pixelX / (window.Width - 1f), 1f - pixelY / (window.Height - 1f));
		}
	}
}
