using DMS.Application;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace Example
{
	class MyWindow
	{
		private MyWindow()
		{
			visual = new MainVisual();
			globalTime.Start();
		}

		private void Render()
		{
			float time = (float)globalTime.Elapsed.TotalSeconds;
			if(doPostProcessing)
			{
				visual.DrawWithPostProcessing(time);
			}
			else
			{
				visual.Draw();
			}
		}

		private Stopwatch globalTime = new Stopwatch();
		private MainVisual visual;
		private bool doPostProcessing;

		private void Update(float updatePeriod)
		{
			doPostProcessing = Keyboard.GetState().IsKeyDown(Key.Space);
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var window = new MyWindow();
			app.Resize += window.visual.Resize;
			app.GameWindow.ConnectMouseEvents(window.visual.OrbitCamera);
			app.Render += window.Render;
			app.Update += window.Update;
			app.Run();
		}
	}
}