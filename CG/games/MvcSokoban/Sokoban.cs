using DMS.Application;
using DMS.Base;
using System;
using System.IO;
using System.Windows.Forms;

namespace MvcSokoban
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			GameLogic logic;
			try
			{
				logic = (GameLogic)Serialize.ObjFromBinFile(GetGameStateFilePath()); //try to load the game state from a file at start of program
			}
			catch
			{
				logic = new GameLogic(); //loading failed -> reset
			}
			app.GameWindow.Title = "Sokoban";
			app.GameWindow.CursorVisible = false;
			app.GameWindow.Closing += (s, e) => logic.ObjIntoBinFile(GetGameStateFilePath()); //save game state at end of program

			var renderer = new Renderer();

			var game = new SceneGame(logic, renderer);
			var menu = new SceneMenu(logic, renderer);
			IScene scene = game;
			app.GameWindow.KeyDown += (s, e) => 
			{
				if (!scene.HandleInput(e.Key))
				{
					if (scene == game)
					{
						scene = menu;
						logic.ResetLevel();
					}
					else if (scene == menu)
					{
						scene = game;
					}
				}
			};
			app.Resize += (w, h) => renderer.ResizeWindow(w, h);
			app.Render += () => scene.Render();
			app.Run();
		}

		private static string GetGameStateFilePath()
		{
			return Path.ChangeExtension(Application.ExecutablePath, ".gameState");
		}
	}
}