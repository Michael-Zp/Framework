using DMS.Application;
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
		private static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run(60.0);
		}

		private MyApplication()
		{
			var mode = new GraphicsMode(new ColorFormat(32), 24, 8, 0);
			gameWindow = new GameWindow(700, 700, mode, "Example", GameWindowFlags.Default, DisplayDevice.Default, 4, 3, GraphicsContextFlags.ForwardCompatible);

			model = new Model();
			visual = new MainVisual();

			gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.ConnectMouseEvents(visual.Camera);
			gameWindow.KeyDown += (s, arg) => gameWindow.Close();
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
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