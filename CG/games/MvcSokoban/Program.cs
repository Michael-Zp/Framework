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
			var logic = new GameLogic();
			var visual = new Visual();
			app.GameWindow.KeyDown += (s, e) =>
			{
				switch (e.Key)
				{
					case Key.Escape: app.GameWindow.Close(); break;
					case Key.R: logic.ResetLevel(); break;
					case Key.N: logic.NextLevel(); break;
					case Key.B: logic.Undo(); break;
					case Key.Left: logic.Update(LevelLogic.Movement.LEFT); break;
					case Key.Right: logic.Update(LevelLogic.Movement.RIGHT); break;
					case Key.Up: logic.Update(LevelLogic.Movement.UP); break;
					case Key.Down: logic.Update(LevelLogic.Movement.DOWN); break;
				};
			};
			visual.ResizeWindow(app.GameWindow.Width, app.GameWindow.Height);
			app.GameWindow.Resize += (s, e) => visual.ResizeWindow(app.GameWindow.Width, app.GameWindow.Height);
			app.GameWindow.Visible = true;
			while (app.GameWindow.Exists)
			{
				visual.DrawScreen(logic.GetLevel());
				app.GameWindow.SwapBuffers();
				app.GameWindow.ProcessEvents();
			}
		}
	}
}