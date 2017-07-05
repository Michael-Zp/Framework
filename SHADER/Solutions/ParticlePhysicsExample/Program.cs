using DMS.Application;
using OpenTK.Input;
using System;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			Resources.LoadResources(app.ResourceManager);
			var visual = new MainVisual();
			Action<MouseEventArgs> updateMouseState = (a) => 
			{
				var mouseState = new MouseState();
				mouseState.position = app.CalcNormalized(a.X, a.Y);
				mouseState.drawState = GetDrawState(a.Mouse);
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
