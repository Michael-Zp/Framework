using DMS.Application;
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
			app.GameWindow.MouseMove += (s, a) => visual.MousePosition = app.CalcNormalized(a.X, a.Y);
			app.Render += visual.Render;
			app.Run();
		}
	}
}
