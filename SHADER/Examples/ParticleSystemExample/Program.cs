using DMS.Application;
using System;
using System.Diagnostics;

namespace Example
{
	class MyWindow : IWindow
	{
		public void Render()
		{
			visual.Render();
		}

		public void Update(float updatePeriod)
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
			app.Run(window);
		}
	}
}