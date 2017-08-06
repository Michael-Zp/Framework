using System.Drawing;

namespace MvcSokoban
{
	public class SceneGame : IScene
	{
		public SceneGame(GameLogic logic, Renderer renderer)
		{
			this.logic = logic;
			this.renderer = renderer;
		}

		public bool HandleInput(GameKey key)
		{
			switch (key)
			{
				case GameKey.Menu: return false;
				case GameKey.Reset: logic.ResetLevel(); break;
				case GameKey.Back: logic.Undo(); break;
				case GameKey.Left: logic.Update(Movement.LEFT); break;
				case GameKey.Right: logic.Update(Movement.RIGHT); break;
				case GameKey.Up: logic.Update(Movement.UP); break;
				case GameKey.Down: logic.Update(Movement.DOWN); break;
			};
			return true;
		}

		public void Render()
		{
			renderer.Clear();
			renderer.DrawLevel(logic.GetLevel(), Color.White);
			renderer.Print(logic.LevelNr + "/" + logic.Moves, .05f);
		}

		private readonly GameLogic logic;
		private readonly Renderer renderer;
	}
}
