using DMS.OpenGL;
using DMS.System;
using LevelData;
using OpenTK.Input;
using System;
using System.IO;

namespace Example
{
	//todo: add logic; read logic data from level;
	class MyController : IWindow
	{
		private Level level = null;
		private GameLogic logic = new GameLogic();
		private Renderer renderer = new Renderer();

		public MyController()
		{
			logic.NewPosition += Logic_NewPosition;
			LoadLevelData(LevelData.level1);
		}

		public void Render()
		{
			renderer.Render(logic.Bounds);
		}

		public void Update(float updatePeriod)
		{
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			float axisUpDown = Keyboard.GetState()[Key.Down] ? -1.0f : Keyboard.GetState()[Key.Up] ? 1.0f : 0.0f;
			logic.Update(updatePeriod, axisLeftRight, axisUpDown);
		}

		private void LoadLevelData(byte[] levelData)
		{
			using (var stream = new MemoryStream(levelData))
			{
				level = Serialize.ObjFromBinStream(stream) as Level;
				//set level bounds
				logic.Bounds = level.Bounds;
				//load colliders
				foreach (var collider in level.colliders)
				{
					logic.AddCollider(collider.Name, collider.Bounds);
				}
				//load sprites
				foreach (var sprite in level.Sprites)
				{
					renderer.AddSprite(sprite.Name, sprite.Layer, sprite.RenderBounds, sprite.TextureName, sprite.Texture);
				}
			}
		}

		private void Logic_NewPosition(string name, float x, float y)
		{
			renderer.UpdateSprites(name, x, y);
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var controller = new MyController();
			app.GameWindow.Resize += (s, e) => controller.renderer.Resize(app.GameWindow.Width, app.GameWindow.Height);
			app.Run(controller);
		}
	}
}