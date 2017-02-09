using Framework;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	/// <summary>
	/// Shows occlusion queries in action
	/// </summary>
	class MyApplication
	{
		private GameWindow gameWindow;
		float moveDelta = 0.01f;
		private Box2D boxA = new Box2D(-.2f, -.2f, .4f, .4f);
		private Box2D boxB = new Box2D(-.5f, -.1f, .2f, .2f);
		private QueryObject queryA, queryB;

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
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();

			queryA = new QueryObject();
			queryB = new QueryObject();

			//for query to work
			GL.Enable(EnableCap.DepthTest);
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			moveDelta = (boxB.CenterX > 0.5f) ? -Math.Abs(moveDelta) : (boxB.CenterX < -0.5f) ? Math.Abs(moveDelta) : moveDelta;

			boxB.X += moveDelta;
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Color3(Color.White);
			queryA.Activate(QueryTarget.SamplesPassed);
			DrawBox(boxA, 0.0f);
			queryA.Deactivate();

			GL.Color3(Color.Red);
			queryB.Activate(QueryTarget.SamplesPassed);
			DrawBox(boxB, 0.5f);
			queryB.Deactivate();

			Console.WriteLine(queryA.Result);
			Console.WriteLine(queryB.Result);

		}

		private void DrawBox(Box2D rect, float depth)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex3(rect.X, rect.Y, depth);
			GL.Vertex3(rect.MaxX, rect.Y, depth);
			GL.Vertex3(rect.MaxX, rect.MaxY, depth);
			GL.Vertex3(rect.X, rect.MaxY, depth);
			GL.End();
		}
	}
}