using Zenseless.Application;
using Zenseless.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using Zenseless.Base;

namespace MiniGalaxyBirds
{

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var renderer = new Renderer();
			LoadResources(renderer);
			GameLogic gameLogic = new GameLogic(renderer);

			GameTime time = new GameTime();

			window.Update += (t) => HandleInput(gameLogic, time.AbsoluteTime);
			window.Resize += renderer.ResizeWindow;
			window.Render += () => renderer.DrawScreen(GameLogic.visibleFrame, gameLogic.Points);
			window.Render += () => time.NewFrame();
			window.Render += () => window.GameWindow.Title = $"{time.FPS}FPS";

			window.Run();
		}

		private static void LoadResources(Renderer renderer)
		{
			//private static TextureFont font = new TextureFont("media/bitmap_fonts/OpenTKTextureFont.png", 16, 0, 0.8f, 0.8f, 0.8f);
			//private static TextureFont font = new TextureFont("media/bitmap_fonts/Orange with Shadow.png", 10, 32, 1.0f, 1.0f, 0.9f);
			//private static TextureFont font = new TextureFont("media/bitmap_fonts/LED Green.png", 10, 32, 0.9f, 0.7f, 0.8f);
			//private static TextureFont font = new TextureFont("media/bitmap_fonts/Bamboo.png", 10, 32, 0.8f, 0.7f, 1.0f);
			renderer.RegisterFont(new TextureFont(TextureLoader.FromBitmap(Resourcen.Video_Phreak), 10, 32));
			renderer.Register("player", TextureLoader.FromBitmap(Resourcen.blueships1));
			renderer.Register("enemy", TextureLoader.FromBitmap(Resourcen.redship4));
			renderer.Register("bulletPlayer", TextureLoader.FromBitmap(Resourcen.blueLaserRay));
			renderer.Register("bulletEnemy", TextureLoader.FromBitmap(Resourcen.redLaserRay));
			renderer.Register("explosion", TextureLoader.FromBitmap(Resourcen.explosion));
			renderer.Register("background", TextureLoader.FromBitmap(Resourcen.background));
			renderer.CreateDrawable("background", GameLogic.visibleFrame);
		}

		private static void HandleInput(GameLogic gameLogic, float time)
		{
			float axisUpDown = Keyboard.GetState()[Key.Up] ? -1.0f : Keyboard.GetState()[Key.Down] ? 1.0f : 0.0f;
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];
			gameLogic.Update(time, axisUpDown, axisLeftRight, shoot);
		}
	}
}