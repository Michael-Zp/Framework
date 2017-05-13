using DMS.Application;
using DMS.Base;
using System;
using System.IO;
using System.Windows.Forms;

namespace Example
{
	class MyWindow
	{
		private GameState gameState;

		public MyWindow()
		{
			try
			{
				gameState = (GameState)Serialize.ObjFromBinFile(GetGameStateFilePath()); //try to load from file
			}
			catch
			{
				gameState = new GameState(); //loading failed -> reset
			}
		}

		private void Save()
		{
			gameState.ObjIntoBinFile(GetGameStateFilePath());
		}

		private void Render()
		{
			Visual.DrawScreen(gameState);
			VisualConsole.DrawScreen(gameState);
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var window = new MyWindow();
			app.GameWindow.Closing += (s, e) => window.Save();
			app.GameWindow.MouseDown += (s, e) =>
			{
				var coord = app.CalcNormalized(e.X, e.Y);
				window.HandleInput((int)e.Button, coord.X, coord.Y);
			};
			app.Render += window.Render;
			app.Run();
		}

		private void HandleInput(int button, float x, float y)
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