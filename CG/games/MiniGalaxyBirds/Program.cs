using DMSOpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace MiniGalaxyBirds
{
	class MyApplication
	{
		public MyApplication()
		{
			gameWindow = new GameWindow();
			gameWindow.Load += GameWindow_Load;
			gameWindow.Resize += GameWindow_Resize;
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => { gameWindow.SwapBuffers(); };

			renderer = new Renderer();
			//private static TextureFont font = new TextureFont("../../media/bitmap_fonts/OpenTKTextureFont.png", 16, 0, 0.8f, 0.8f, 0.8f);
			//private static TextureFont font = new TextureFont("../../media/bitmap_fonts/Orange with Shadow.png", 10, 32, 1.0f, 1.0f, 0.9f);
			//private static TextureFont font = new TextureFont("../../media/bitmap_fonts/LED Green.png", 10, 32, 0.9f, 0.7f, 0.8f);
			//private static TextureFont font = new TextureFont("../../media/bitmap_fonts/Bamboo.png", 10, 32, 0.8f, 0.7f, 1.0f);
			renderer.RegisterFont(new TextureFont(TextureLoader.FromBitmap(Resourcen.Video_Phreak), 10, 32));
			renderer.Register("player", TextureLoader.FromBitmap(Resourcen.blueships1));
			renderer.Register("enemy", TextureLoader.FromBitmap(Resourcen.redship4));
			renderer.Register("bulletPlayer", TextureLoader.FromBitmap(Resourcen.blueLaserRay));
			renderer.Register("bulletEnemy", TextureLoader.FromBitmap(Resourcen.redLaserRay));
			renderer.Register("explosion", TextureLoader.FromBitmap(Resourcen.explosion));

			this.galaxyBirds = new GameLogic(renderer);
			timeSource.Start();
		}

		[STAThread]
		public static void Main()
		{
			var myApp = new MyApplication();
			myApp.gameWindow.Run(60.0, 60.0);
		}

		private void GameWindow_Load(object sender, EventArgs e)
		{
			//gameWindow.WindowBorder = WindowBorder.Hidden;
			//gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.VSync = VSyncMode.On;
		}

		private void GameWindow_Resize(object sender, EventArgs e)
		{
			renderer.ResizeWindow(gameWindow.Width, gameWindow.Height);
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (Keyboard.GetState()[Key.Escape])
			{
				gameWindow.Exit();
			}
			float axisUpDown = Keyboard.GetState()[Key.Up] ? -1.0f : Keyboard.GetState()[Key.Down] ? 1.0f : 0.0f;
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];
			galaxyBirds.Update((float)timeSource.Elapsed.TotalSeconds, axisUpDown, axisLeftRight, shoot);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			renderer.DrawScreen(GameLogic.visibleFrame, galaxyBirds.Points);
		}

		private GameWindow gameWindow;
		private Renderer renderer;
		private GameLogic galaxyBirds;
		private Stopwatch timeSource = new Stopwatch();
	}
}