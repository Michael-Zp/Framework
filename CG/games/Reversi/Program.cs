using DMS.Application;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Reversi
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var logic = new GameLogic();
			var visual = new Visual();
			app.GameWindow.Closing += (s, e) => { /*todo: save the game state */ };
			app.Resize += visual.Resize;
			app.Render += () => visual.Render(logic.GameState);
			app.GameWindow.MouseDown += (s, e) =>
			{
				if (e.Button != MouseButton.Left) return; //only accept left mouse button
				var coord = app.CalcNormalized(e.X, e.Y); //convert mouse coordinates from pixel to [0,1]²
				var gridPos = visual.CalcGridPosFromNormalized(coord);
				logic.Move(gridPos); //do move
			};
			app.Run();
		}
	}
}