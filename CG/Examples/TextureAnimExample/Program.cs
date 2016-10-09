using Framework;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private SpriteSheetAnimation explosion;
		private AnimationTextures alienShip;
		private Stopwatch timeSource = new Stopwatch();

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			//setup
			gameWindow = new GameWindow(700, 700);
			gameWindow.KeyDown += (s, arg) => gameWindow.Close();
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			//animation using a single SpriteSheet
			explosion = new SpriteSheetAnimation(new SpriteSheet(TextureLoader.FromBitmap(Resourcen.explosion), 5), 0, 24, 1);
			//animation using a bitmap for each frame
			alienShip = new AnimationTextures(.5f);
			//art from http://millionthvector.blogspot.de/p/free-sprites.html
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10001));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10002));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10003));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10004));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10005));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10006));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10007));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10008));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10009));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10010));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10011));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10012));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10013));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10014));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10015));

			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//start game time
			timeSource.Start();
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);

			GL.Enable(EnableCap.Blend); // for transparency in textures
			explosion.Draw(new Box2D(-.7f, -.2f, .4f, .4f), (float)timeSource.Elapsed.TotalSeconds);

			alienShip.Draw(new Box2D(.3f, -.2f, .4f, .4f), (float)timeSource.Elapsed.TotalSeconds);
			GL.Disable(EnableCap.Blend); // for transparency in textures
		}
	}
}