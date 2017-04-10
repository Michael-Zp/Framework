using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private MainVisual visual;
		private Model model;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run(60.0);
		}

		private MyApplication()
		{
			var mode = new GraphicsMode(new ColorFormat(32), 24, 8, 0);
			gameWindow = new GameWindow(700, 700, mode, "Example", GameWindowFlags.Default, DisplayDevice.Default, 4, 3, GraphicsContextFlags.ForwardCompatible);

			gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.MouseMove += GameWindow_MouseMove;
			gameWindow.MouseWheel += GameWindow_MouseWheel;
			gameWindow.KeyDown += (s, arg) => gameWindow.Close();
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			model = new Model();
			visual = new MainVisual();
		}

		private void GameWindow_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			visual.Camera.Distance -= 10 * e.DeltaPrecise;
		}

		private void GameWindow_MouseMove(object sender, MouseMoveEventArgs e)
		{
			if (ButtonState.Pressed == e.Mouse.LeftButton)
			{
				visual.Camera.Azimuth += 300 * e.XDelta / (float)gameWindow.Width;
				visual.Camera.Elevation += 300 * e.YDelta / (float)gameWindow.Height;
			}
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			model.Update();
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			visual.Render(model.Bodies);
		}
	}
}