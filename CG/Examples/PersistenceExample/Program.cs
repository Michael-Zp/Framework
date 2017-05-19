using DMS.Application;
using DMS.Base;
using System;
using System.IO;
using System.Windows.Forms;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			GameState gameState;
			try
			{
				gameState = (GameState)Serialize.ObjFromBinFile(GetGameStateFilePath()); //try to load the game state from a file
			}
			catch
			{
				gameState = new GameState(); //loading failed -> reset
			}

			app.GameWindow.Closing += (s, e) => gameState.ObjIntoBinFile(GetGameStateFilePath());
			app.GameWindow.MouseDown += (s, e) =>
			{
				var coord = app.CalcNormalized(e.X, e.Y);
				HandleInput(gameState, (int)e.Button, coord.X, coord.Y);
			};
			app.Render += () => Visual.DrawScreen(gameState); //this draws the game using OpenGL
			//app.Render += () => VisualConsole.DrawScreen(gameState); //this draws the game to the console
			app.Run();
		}

		private static void HandleInput(GameState gameState, int button, float x, float y)
		{
			//transform normalized coordinates to grid coordinates
			var gridX = (int)(x * gameState.GridWidth);
			var gridY = (int)(y * gameState.GridHeight);
			FieldType field;
			switch (button)
			{
				case 0:
					field = FieldType.CROSS;
					break;
				case 1:
					field = FieldType.DIAMONT;
					break;
				default:
					field = FieldType.EMPTY;
					break;
			}
			gameState[gridX, gridY] = field;
		}

		private static string GetGameStateFilePath()
		{
			return Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "gameState.bin";
		}
	}
}