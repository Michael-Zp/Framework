using DMS.Application;
using System;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var visual = new MainVisual();
			app.GameWindow.ConnectEvents(visual.OrbitCamera);
			app.Render += visual.Render;
			app.Run();
		}
	}
}