using DMS.Application;
using System;
using System.Diagnostics;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var controller = new Controller();
			var visual = new MainVisual();
			var timeSource = new Stopwatch();
			app.GameWindow.ConnectMouseEvents(visual.OrbitCamera);
			app.Render += visual.Render;
			app.Update += (t) => visual.Update((float)timeSource.Elapsed.TotalSeconds);
			timeSource.Start();
			app.Run();
		}
	}
}