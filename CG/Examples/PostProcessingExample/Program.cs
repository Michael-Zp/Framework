using Framework;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow(1024, 1024);
		private PostProcessing postProcessing;
		private Stopwatch globalTime = new Stopwatch();
		private Box2D player = new Box2D(0.0f, -0.95f, 0.3f, 0.3f);
		private Texture texPlayer;
		private Box2D background = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private Texture texBackground;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.KeyDown += GameWindow_KeyDown;

			texBackground = TextureLoader.FromBitmap(Resources.background);
			texPlayer = TextureLoader.FromBitmap(Resources.bird);

			postProcessing = new PostProcessing(gameWindow.Width, gameWindow.Height);
			//postProcessing.SetShader(Encoding.UTF8.GetString(Resources.quickBlur)); postProcessing.GenerateMipMap = true;
			postProcessing.SetShader(Encoding.UTF8.GetString(Resources.Sepia));

			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);

			globalTime.Start();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key)
			{
				gameWindow.Exit();
			}
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			float updatePeriod = (float)gameWindow.UpdatePeriod;

			//player movement
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			player.X += updatePeriod * axisLeftRight;
			float axisupDown = Keyboard.GetState()[Key.Down] ? -1.0f : Keyboard.GetState()[Key.Up] ? 1.0f : 0.0f;
			player.Y += updatePeriod * axisupDown;

			//limit player position [left, right]
			player.PushXRangeInside(background);
			player.PushYRangeInside(background);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			postProcessing.Start();

			GL.Color3(Color.White);
			//draw background
			texBackground.BeginUse();
			background.DrawTexturedRect(Box2D.BOX01);
			texBackground.EndUse();

			//draw player
			texPlayer.BeginUse();
			player.DrawTexturedRect(Box2D.BOX01);
			texPlayer.EndUse();

			postProcessing.EndAndApply(gameWindow.Width, gameWindow.Height, (float)globalTime.Elapsed.TotalSeconds);
		}
	}
}