using DMS.Application;
using DMS.Base;
using OpenTK.Input;
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
			app.GameWindow.KeyDown += (s, e) =>
			{
				switch (e.Key)
				{
					case Key.Escape: app.GameWindow.Close(); break;
					case Key.R: logic.ResetLevel(); break;
					case Key.N: ++logic.LevelNr; break;
					case Key.P: --logic.LevelNr; break;
					case Key.BackSpace: logic.Undo(); break;
					case Key.Left: logic.Update(Movement.LEFT); break;
					case Key.Right: logic.Update(Movement.RIGHT); break;
					case Key.Up: logic.Update(Movement.UP); break;
					case Key.Down: logic.Update(Movement.DOWN); break;
				};
			};
			var visual = new Renderer();
			app.Resize += (w, h) => visual.ResizeWindow(w, h);
			app.Render += () => visual.DrawScreen(logic.GetLevel(), logic.LevelNr + "/" + logic.Moves);
			app.Run();
		}

		private static string GetGameStateFilePath()
		{
			return Path.ChangeExtension(Application.ExecutablePath, ".gameState");
		}
	}
}