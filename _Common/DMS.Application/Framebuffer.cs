using OpenTK;

namespace DMS.Application
{
	public class Framebuffer
	{
		public int Height { get { return gameWindow.Height; } }
		public int Width { get { return gameWindow.Width; } }

		public Framebuffer(int width = 512, int height = 512)
		{
			gameWindow = new GameWindow(width, height);
			gameWindow.Visible = true;
		}

		public bool NextFrame()
		{
			gameWindow.SwapBuffers(); //double buffering
			gameWindow.ProcessEvents(); //handle all events that are sent to the window (user inputs, operating system stuff); this call could destroy window, so check immediatily after this call if window still exists, otherwise gl calls will fail.
			return gameWindow.Exists;
		}

		private GameWindow gameWindow;
	}
}
