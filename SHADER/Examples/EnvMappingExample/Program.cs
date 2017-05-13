using DMS.Application;
using System;

namespace Example
{
	class MyApplication : ExampleApplication
	{
		private MainVisual visual;

		private MyApplication()
		{
			visual = new MainVisual();
			Resize += (width, height) => visual.OrbitCamera.Aspect = (float)width / height;
			GameWindow.ConnectMouseEvents(visual.OrbitCamera);
		}

		[STAThread]
		private static void Main()
		{
			var app = new MyApplication();
			app.Render += app.visual.Render;
			app.Update += app.visual.Update;
			app.Run();
		}
	}
}