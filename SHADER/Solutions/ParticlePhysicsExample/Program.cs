using Zenseless.Application;
using OpenTK.Input;
using System;
using Zenseless.OpenGL;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			Resources.LoadResources(app.ResourceManager);
			var visual = new MainVisual(app.RenderContext);
			Action<MouseEventArgs> updateMouseState = (e) => 
			{
				var pos = app.GameWindow.ConvertWindowPixelCoords(e.X, e.Y); //convert pixel coordinates to [0,1]²
				pos *= .5f;
				pos += System.Numerics.Vector2.One * .5f;
				var mouseState = new MouseState()
				{
					position = pos,
					drawState = GetDrawState(e.Mouse)
				};
				visual.MouseState = mouseState;
			};

			app.GameWindow.MouseMove += (s, a) => updateMouseState(a);
			app.GameWindow.MouseDown += (s, a) => updateMouseState(a);
			app.Render += visual.Render;
			app.Run();
		}

		private static int GetDrawState(OpenTK.Input.MouseState mouse)
		{
			if (mouse.IsButtonDown(MouseButton.Left))
			{
				return 1;
			}
			else if (mouse.IsButtonDown(MouseButton.Right))
			{
				return 2;
			}
			return 0;
		}
	}
}
