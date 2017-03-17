using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Example
{
	static class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			GameWindow gameWindow = new GameWindow();
			gameWindow.VSync = VSyncMode.On;
			//register callback for resizing of window
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			var window = new MyWindow();
			//register callback for keyboard
			gameWindow.KeyDown += (sender, e) => { if (OpenTK.Input.Key.Escape == e.Key) gameWindow.Exit(); };
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += (sender, e) => window.Update((float)gameWindow.TargetUpdatePeriod);
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += (sender, e) => window.Render();
			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			//run the update loop, which calls our registered callbacks
			gameWindow.Run(60, 60);
		}
	}
}