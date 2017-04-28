﻿using DMS.OpenGL;
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
			GameWindow.Resize += GameWindow_Resize;
			visual = new MainVisual();
		}

		private void GameWindow_Resize(object sender, EventArgs e)
		{
			visual.OrbitCamera.Aspect = (float)GameWindow.Width / GameWindow.Height;
		}

		private void Run()
		{
			Run(visual);
		}

		private void GameWindow_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			visual.OrbitCamera.FovY *= (float)Math.Pow(1.05, e.DeltaPrecise);
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
		private static void Main()
		{
			var app = new MyApplication();
			//app.GameWindow.WindowState = OpenTK.WindowState.Fullscreen;
			app.Run();
		}
	}
}