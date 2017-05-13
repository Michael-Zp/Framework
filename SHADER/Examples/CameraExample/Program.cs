using DMS.Application;
using System;

namespace Example
{
	class MyApplication
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var visual = new MainVisual();
			app.GameWindow.ConnectMouseEvents(visual.OrbitCamera);
			app.Render += visual.Render;
			app.Resize += (width, height) => visual.OrbitCamera.Aspect = (float)width / height;
			app.Run();
		}
	}
}