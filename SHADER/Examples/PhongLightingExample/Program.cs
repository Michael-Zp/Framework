using DMS.OpenGL;
using OpenTK.Input;
using System;

namespace Example
{
	class MyApplication : ExampleApplication
	{
		private MainVisual visual;

		private MyApplication()
		{
			GameWindow.MouseMove += GameWindow_MouseMove;
			GameWindow.MouseWheel += GameWindow_MouseWheel;
			visual = new MainVisual();
		}

		private void Run()
		{
			Run(visual);
		}

		private void GameWindow_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			visual.OrbitCamera.Distance *= (float)Math.Pow(1.05, e.DeltaPrecise);
		}

		private void GameWindow_MouseMove(object sender, MouseMoveEventArgs e)
		{
			if (ButtonState.Pressed == e.Mouse.LeftButton)
			{
				visual.OrbitCamera.Azimuth += 300 * e.XDelta / (float)GameWindow.Width;
				visual.OrbitCamera.Elevation += 300 * e.YDelta / (float)GameWindow.Height;
			}
		}

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.Run();
		}
	}
}