using DMS.Application;
using OpenTK.Input;
using System;

namespace Reversi
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var logic = new GameLogic(); //todo student: load the game state
			var view = new View();
			window.GameWindow.Closing += (s, e) => { /*todo student: save the game state */ };
			window.Resize += (w, h) => view.Resize(logic.GameState, w, h);
			window.Render += () =>
			{
				var score = "white:" + logic.CountWhite.ToString() + " black:" + logic.CountBlack.ToString(); ;
				window.GameWindow.Title = score;
				view.Render(logic.GameState);
				if (GameLogic.Result.PLAYING != logic.CurrentGameResult)
				{
					var winner = logic.CurrentGameResult.ToString();
					view.PrintMessage(winner);
				}
			};
			window.GameWindow.MouseDown += (s, e) =>
			{
				if (e.Button != MouseButton.Left) return; //only accept left mouse button
				var coord = window.CalcNormalized(e.X, e.Y); //convert mouse coordinates from pixel to [0,1]²
				var gridPos = view.CalcGridPosFromNormalized(new OpenTK.Vector2(coord.X, coord.Y)); //convert normalized mouse coordinates into grid coordinates
				logic.Move(gridPos); //do move
			};
			window.Run();
		}
	}
}