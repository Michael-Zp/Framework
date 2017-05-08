using DMS.Base;
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
			gameWindow.Resize += GameWindow_Resize;
			//register callback for keyboard
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.KeyDown += (sender, e) => { if (Key.Escape == e.Key) gameWindow.Exit(); };
		}

		public IGameWindow GameWindow { get { return gameWindow; } }

		public bool IsRecording
		{
			get { return !ReferenceEquals(null, frameListCreator); }
			set
			{
				if (!ReferenceEquals(null, frameListCreator)) return;
				frameListCreator = value ? new FrameListCreator(gameWindow.Width, gameWindow.Height) : null;
			}
		}

		public Vector2 CalcNormalized(int pixelX, int pixelY)
		{
			return new Vector2(pixelX / (gameWindow.Width - 1f), 1f - pixelY / (gameWindow.Height - 1f));
		}

		public void Run(IWindow window)
		{
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += (sender, e) => window.Update((float)gameWindow.TargetUpdatePeriod);
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += (sender, e) => Render(window);
			//run the update loop, which calls our registered callbacks
			gameWindow.Run(60, 60);
		}

		private GameWindow gameWindow;
		private FrameListCreator frameListCreator;

		private void Render(IWindow window)
		{
			//record frame
			frameListCreator?.Activate();

			window.Render();
			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.SwapBuffers();

			//stop recording frame
			frameListCreator?.Deactivate();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					if (!ReferenceEquals(null, frameListCreator)) frameListCreator.Frames.SaveToDefaultDir();
					gameWindow.Exit();
					break;
				case Key.F11:
					gameWindow.WindowState = WindowState.Fullscreen == gameWindow.WindowState ? WindowState.Normal : WindowState.Fullscreen;
					break;
			}
		}

		private void GameWindow_Resize(object sender, global::System.EventArgs e)
		{
			GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			if (!ReferenceEquals(null, frameListCreator))
			{
				frameListCreator.Dispose();
				frameListCreator = new FrameListCreator(gameWindow.Width, gameWindow.Height);
			}
		}
	}
}
