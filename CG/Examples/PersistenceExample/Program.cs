using Framework;
using OpenTK;
using OpenTK.Input;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private GameState gameState;

		public MyApplication()
		{
			try
			{
				gameState = (GameState)Serialize.ObjFromBinFile(GetGameStateFilePath()); //try to load from file
			}
			catch
			{
				gameState = new GameState(); //loading failed -> reset
			}
			gameWindow = new GameWindow();
			gameWindow.Closing += GameWindow_Closing;
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.MouseDown += GameWindow_MouseDown;
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
		}

		[STAThread]
		public static void Main()
		{
			MyApplication app = new MyApplication();
			app.gameWindow.Run(60.0);
		}

		private void GameWindow_Closing(object sender, CancelEventArgs e)
		{
			gameState.ObjIntoBinFile(GetGameStateFilePath());
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key)
			{
				gameWindow.Exit();
			}
		}

		private void GameWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//transform window coordinates to grid coordinates
			var gridX = (e.X * gameState.GridWidth) / (gameWindow.Width - 1);
			var gridY = (e.Y * gameState.GridHeight) / (gameWindow.Height - 1);

			FieldType field;
			switch (e.Button)
			{
				case MouseButton.Left:
					field = FieldType.CROSS;
					break;
				case MouseButton.Right:
					field = FieldType.DIAMONT;
					break;
				default:
					field = FieldType.EMPTY;
					break;
			}
			gameState[gridX, gridY] = field;
		}

		void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			Visual.DrawScreen(gameState);
			VisualConsole.DrawScreen(gameState);
		}

		private static string GetGameStateFilePath()
		{
			return Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "gameState.bin";
		}
	}
}