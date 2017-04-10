using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private MainVisual visual;
		private Stopwatch timeSource = new Stopwatch();

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			gameWindow = new GameWindow(800, 800);
			//gameWindow.LoadLayout();
			//gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.MouseMove += GameWindow_MouseMove;
			gameWindow.MouseWheel += GameWindow_MouseWheel;
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += (s, arg) => visual.Render();			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			gameWindow.UpdateFrame += (s, arg) => visual.Update((float)timeSource.Elapsed.TotalSeconds);
			visual = new MainVisual();
			timeSource.Start();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape: gameWindow.Close(); break;
			}
		}

		private void GameWindow_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			visual.OrbitCamera.Distance -= e.DeltaPrecise;
		}

		private void GameWindow_MouseMove(object sender, MouseMoveEventArgs e)
		{
			if (ButtonState.Pressed == e.Mouse.LeftButton)
			{
				visual.OrbitCamera.Azimuth += 300 * e.XDelta / (float)gameWindow.Width;
				visual.OrbitCamera.Elevation += 300 * e.YDelta / (float)gameWindow.Height;
			}
		}
	}
}