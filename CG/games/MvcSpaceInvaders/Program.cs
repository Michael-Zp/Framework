using DMS.OpenGL;
using OpenTK;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace MvcSpaceInvaders
{
	class MyApplication
	{
		public MyApplication()
		{
			gameWindow = new GameWindow();
			logic = new GameLogic();
			Texture texPlayer = TextureLoader.FromBitmap(Resourcen.blueships1);
			Texture texEnemy = TextureLoader.FromBitmap(Resourcen.redship4);
			Texture texBullet = TextureLoader.FromBitmap(Resourcen.blueLaserRay);
			visual = new Visual(texEnemy, texBullet, texPlayer);
			sound = new Sound2();
			logic.OnShoot += (sender, args) => { sound.Shoot(); };
			logic.OnEnemyDestroy += (sender, args) => { sound.DestroyEnemy(); };
			logic.OnLost += (sender, args) => { sound.Lost(); gameWindow.Exit(); };

			gameWindow.Load += GameWindow_Load;
			gameWindow.Resize += GameWindow_Resize;
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => { gameWindow.SwapBuffers(); };
			sound.Background();
			timeSource.Start();
			gameWindow.Run(60.0);
		}

		[STAThread]
		public static void Main()
		{
			MyApplication app = new MyApplication();
		}

		void GameWindow_Load(object sender, EventArgs e)
		{
			//fullscreen
			gameWindow.WindowBorder = WindowBorder.Hidden;
			gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.VSync = VSyncMode.On;
		}

		void GameWindow_Resize(object sender, EventArgs e)
		{
			visual.Resize(gameWindow.Width, gameWindow.Height);
		}

		void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (Keyboard.GetState()[Key.Escape])
			{
				gameWindow.Exit();
			}

			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];

			logic.Update((float)timeSource.Elapsed.TotalSeconds, axisLeftRight, shoot);
		}

		void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			visual.DrawScreen(logic.Enemies, logic.Bullets, logic.Player);
		}

		private GameWindow gameWindow;
		private GameLogic logic;
		private Visual visual;
		private Sound2 sound;
		private Stopwatch timeSource = new Stopwatch();
	}
}