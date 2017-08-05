using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MvcSokoban
{
	public class SceneMenu : IScene
	{
		public SceneMenu(GameLogic logic, Renderer renderer)
		{
			this.logic = logic;
			this.renderer = renderer;
		}

		public bool HandleInput(Key key)
		{
			switch (key)
			{
				case Key.Right: ++logic.LevelNr; break;
				case Key.Left: --logic.LevelNr; break;
				case Key.Enter: return false;
			};
			return true;
		}

		public void Render()
		{
			renderer.Clear();
			GL.Color3(.5f, .5f, .5f);
			renderer.DrawLevel(logic.GetLevel());
			renderer.Print("Level " + logic.LevelNr, .1f, TextAlignment.Center);
		}

		private readonly GameLogic logic;
		private readonly Renderer renderer;
	}
}
