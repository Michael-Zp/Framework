using DMSOpenGL;
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
		private Box2D bird = Box2dExtensions.CreateFromCenterSize(0.0f, -0.8f, 0.3f, 0.3f);
		private Texture texBird;
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
			texBird = TextureLoader.FromBitmap(Resources.bird);

			postProcessing = new PostProcessing(gameWindow.Width, gameWindow.Height);
			try
			{
				postProcessing.SetShader(Encoding.UTF8.GetString(Resources.EdgeDetect));
			}
			catch (ShaderException e)
			{
				Console.WriteLine(e.Log);
			}

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

			var R = Transform2D.CreateRotationAroundOrigin(2.0f * updatePeriod);
			bird.TransformCenter(R);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			bool doPostProcessing = !Keyboard.GetState()[Key.Space];

			if(doPostProcessing) postProcessing.Start();

			GL.Color3(Color.White);
			//draw background
			texBackground.Activate();
			background.DrawTexturedRect(Box2D.BOX01);
			texBackground.Deactivate();

			//draw player
			texBird.Activate();
			bird.DrawTexturedRect(Box2D.BOX01);
			texBird.Deactivate();

			if (doPostProcessing) postProcessing.EndAndApply(gameWindow.Width, gameWindow.Height, (float)globalTime.Elapsed.TotalSeconds);


		}
	}
}