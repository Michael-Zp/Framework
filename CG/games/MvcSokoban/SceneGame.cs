using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MvcSokoban
{
	public class SceneGame : IScene
	{
		public SceneGame(GameLogic logic, Renderer renderer)
		{
			this.logic = logic;
			this.renderer = renderer;
		}

		public bool HandleInput(Key key)
		{
			switch (key)
			{
				case Key.BackSpace: return false;
				case Key.R: logic.ResetLevel(); break;
				case Key.B: logic.Undo(); break;
				case Key.Left: logic.Update(Movement.LEFT); break;
				case Key.Right: logic.Update(Movement.RIGHT); break;
				case Key.Up: logic.Update(Movement.UP); break;
				case Key.Down: logic.Update(Movement.DOWN); break;
			};
			return true;
		}

		public void Render()
		{
			renderer.Clear();
			GL.Color3(1f, 1f, 1f);
			renderer.DrawLevel(logic.GetLevel());
			renderer.Print(logic.LevelNr + "/" + logic.Moves, .05f);
		}

		private readonly GameLogic logic;
		private readonly Renderer renderer;
	}
}
