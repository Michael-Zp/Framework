using DMS.Application;
using DMS.Geometry;
using OpenTK.Input;
using OpenTK.Platform;
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

		public void Render()
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

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var window = new MyWindow();
			app.Resize += window.visual.Resize;
			app.GameWindow.UpdateFrame += (s, e) =>
			{
				window.doPostProcessing = Keyboard.GetState().IsKeyDown(Key.Space);
			};
			app.GameWindow.ConnectMouseEvents(window.visual.OrbitCamera);
			app.Render += window.Render;
			app.Run();
		}
	}
}