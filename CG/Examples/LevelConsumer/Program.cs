using DMS.Application;
using DMS.Base;
using LevelData;
using OpenTK.Input;
using System;
using System.IO;

namespace Example
{
	class MyController
	{
		private GameLogic logic = new GameLogic();
		private Renderer renderer = new Renderer();

		public MyController()
		{
			logic.NewPosition += (name, x, y) => renderer.UpdateSprites(name, x, y);
			LoadLevelData(LevelData.level1);
		}

		private void Render()
		{
			renderer.Render(logic.Bounds);
		}

		private void Update(float updatePeriod)
		{
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			float axisUpDown = Keyboard.GetState()[Key.Down] ? -1.0f : Keyboard.GetState()[Key.Up] ? 1.0f : 0.0f;
			logic.Update(updatePeriod, axisLeftRight, axisUpDown);
		}

		private void LoadLevelData(byte[] levelData)
		{
			using (var stream = new MemoryStream(levelData))
			{
				Level level = Serialize.ObjFromBinStream(stream) as Level;
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
					renderer.AddSprite(sprite.Name, sprite.Layer, sprite.RenderBounds, sprite.TextureName, sprite.Bitmap);
				}
			}
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var controller = new MyController();
			app.Resize += (width, height) => controller.renderer.Resize(width, height);
			app.Render += controller.Render;
			app.Update += controller.Update;
			app.Run();
		}
	}
}