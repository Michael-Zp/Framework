using DMS.Application;
using OpenTK.Input;
using System;

namespace MvcSokoban
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			uint levelNr = 1;
			var logic = LoadLevel(levelNr);
			var visual = new Visual();
			app.Update += (t) =>
			{
				if (logic.GetLevel().IsWon())
				{
					++levelNr;
					logic = LoadLevel(levelNr);
				}
			};
			app.GameWindow.KeyDown += (s, e) =>
			{
				switch (e.Key)
				{
					case Key.Escape: app.GameWindow.Close(); break;
					case Key.R: logic = LoadLevel(levelNr); break;
					case Key.N: ++levelNr; logic = LoadLevel(levelNr); break;
					case Key.Left: logic.Update(GameLogic.Movement.LEFT); break;
					case Key.Right: logic.Update(GameLogic.Movement.RIGHT); break;
					case Key.Up: logic.Update(GameLogic.Movement.UP); break;
					case Key.Down: logic.Update(GameLogic.Movement.DOWN); break;
				};
			};
			app.Resize += (w, h) => visual.ResizeWindow(w, h);
			app.Render += () => visual.DrawScreen(logic.GetLevel());
			Controller controller = new Controller();
			app.Run();
		}

		private static GameLogic LoadLevel(uint levelNr)
		{
			var levelString = Resourcen.ResourceManager.GetString("level" + levelNr.ToString());
			var level = LevelLoader.FromString(levelString);
			if (ReferenceEquals(null, level)) return null;
			return new GameLogic(level);
		}
	}
}