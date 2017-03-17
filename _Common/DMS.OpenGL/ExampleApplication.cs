using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;

namespace DMS.OpenGL
{
	public class ExampleApplication
	{
		public ExampleApplication()
		{
			gameWindow = new GameWindow();
			gameWindow.VSync = VSyncMode.On;
			//register callback for resizing of window
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			//register callback for keyboard
			gameWindow.KeyDown += (sender, e) => { if (Key.Escape == e.Key) gameWindow.Exit(); };
		}

		public void Run(IWindow window)
		{
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += (sender, e) => window.Update((float)gameWindow.TargetUpdatePeriod);
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += (sender, e) => window.Render();
			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			//run the update loop, which calls our registered callbacks
			gameWindow.Run(60, 60);
		}

		public IGameWindow GameWindow {  get { return gameWindow; } }

		private GameWindow gameWindow;
	}
}
