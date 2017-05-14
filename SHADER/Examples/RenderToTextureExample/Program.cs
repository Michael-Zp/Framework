using DMS.Application;
using OpenTK.Input;
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
			var visual = new MainVisual(app.ResourceManager);

			Stopwatch globalTime = new Stopwatch();
			bool doPostProcessing = false;

			app.Render += () =>
			{
				float time = (float)globalTime.Elapsed.TotalSeconds;
				if (doPostProcessing)
				{
					visual.DrawWithPostProcessing(time);
				}
				else
				{
					visual.Draw();
				}
			};

			app.Update += (t) => doPostProcessing = !Keyboard.GetState()[Key.Space];
			app.Resize += visual.Resize;
			app.GameWindow.ConnectMouseEvents(visual.OrbitCamera);

			globalTime.Start();
			app.Run();
		}
	}
}