using DMS.Application;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace Example
{
	class MyWindow : IWindow
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

		public void Update(float updatePeriod)
		{
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
			app.GameWindow.MouseMove += (s, e) =>
			{
				if (ButtonState.Pressed == e.Mouse.LeftButton)
				{
					window.visual.OrbitCamera.Azimuth += 300 * e.XDelta / (float)app.GameWindow.Width;
					window.visual.OrbitCamera.Elevation += 300 * e.YDelta / (float)app.GameWindow.Height;
				}
			};
			app.GameWindow.MouseWheel += (s, e) =>
			{
				if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
				{
					window.visual.OrbitCamera.FovY *= (float)Math.Pow(1.05, e.DeltaPrecise);
				}
				else
				{
					window.visual.OrbitCamera.Distance *= (float)Math.Pow(1.05, e.DeltaPrecise);
				}
			};
			app.Run(window);
		}
	}
}