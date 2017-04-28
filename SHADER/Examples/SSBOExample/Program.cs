using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private MainVisual visual;

		[STAThread]
		private static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			gameWindow = new GameWindow(800, 800);
			//gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += (s, arg) => visual.Render();			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			visual = new MainVisual();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape: gameWindow.Close(); break;
			}
		}
	}
}