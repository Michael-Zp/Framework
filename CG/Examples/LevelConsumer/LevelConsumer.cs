﻿using LevelData;
using OpenTK.Input;
using System;
using System.IO;
using Zenseless.Application;
using Zenseless.Base;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var controller = new Controller();
			var logic = new GameLogic();
			var renderer = new View();
			logic.NewPosition += (name, x, y) => renderer.UpdateSprites(name, x, y);
			try
			{
				LoadLevelData("level.data", logic, renderer);
				window.Resize += (width, height) => renderer.Resize(width, height);
				window.Render += () => renderer.Render(logic.Bounds);
				window.Update += (updatePeriod) => HandleInput(updatePeriod, logic);
			}
			catch
			{
			}
			window.Run();
		}

		private static void LoadLevelData(string levelFile, GameLogic logic, View renderer)
		{
			using (var stream = new FileStream(levelFile, FileMode.Open))
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

		private static void HandleInput(float updatePeriod, GameLogic logic)
		{
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			float axisUpDown = Keyboard.GetState()[Key.Down] ? -1.0f : Keyboard.GetState()[Key.Up] ? 1.0f : 0.0f;
			logic.Update(updatePeriod, axisLeftRight, axisUpDown);
		}
	}
}