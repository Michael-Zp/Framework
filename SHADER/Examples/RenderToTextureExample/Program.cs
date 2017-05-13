using DMS.Application;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace Example
{
	class MyWindow
	{
		private MyWindow(ExampleApplication app)
		{
			visual = new MainVisual();
			app.Resize += visual.Resize;
			app.GameWindow.ConnectMouseEvents(visual.OrbitCamera);
			globalTime.Start();
		}

		private Stopwatch globalTime = new Stopwatch();
		private MainVisual visual;
		private bool doPostProcessing;

		private void Render()
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
		}

		private void Update(float updatePeriod)
		{
			doPostProcessing = !Keyboard.GetState().IsKeyDown(Key.Space);
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var window = new MyWindow(app);
			app.Render += window.Render;
			app.Update += window.Update;
			app.Run();
		}
	}
}