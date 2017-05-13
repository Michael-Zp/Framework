using DMS.Application;
using System;
using System.Diagnostics;

namespace Example
{
	class MyWindow
	{
		private void Update(float updatePeriod)
		{
			visual.Update((float)timeSource.Elapsed.TotalSeconds);
		}

		private MainVisual visual;
		private Stopwatch timeSource = new Stopwatch();

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var window = new MyWindow();
			window.visual = new MainVisual();
			app.GameWindow.ConnectMouseEvents(window.visual.OrbitCamera);
			window.timeSource.Start();
			app.Render += window.visual.Render;
			app.Update += window.Update;
			app.Run();
		}
	}
}