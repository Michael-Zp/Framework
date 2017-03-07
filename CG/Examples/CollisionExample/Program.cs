using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();
		private Box2D obstacle = new Box2D(-0.2f, 1, 0.4f, 0.2f);
		private Box2D player = new Box2D(0.0f, -0.95f, 0.2f, 0.2f);

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run(60.0f);
		}

		private MyApplication()
		{
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.KeyDown += GameWindow_KeyDown;
			GL.LineWidth(3.0f);
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
			float updatePeriod = (float)gameWindow.TargetUpdatePeriod;

			//player movement
			if(Keyboard.GetState()[Key.Left])
			{
				player.X -= updatePeriod;
			}
			else if (Keyboard.GetState()[Key.Right])
			{
				player.X += updatePeriod;
			}
			//todo: limit player position to visible coordinates
			//todo: let the player also move up down

			//no intersection -> move obstacle
			if (!obstacle.Intersects(player))
			{
				//todo: make the obstacle move toward the player 
				obstacle.Y -= 0.5f * updatePeriod;
			}

			if(obstacle.MaxY < -1)
			{
				obstacle.Y = 1;
			}
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			//clear screen - what happens without?
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.CornflowerBlue);
			DrawComplex(player);
			DrawComplex(obstacle);

			GL.Color3(Color.YellowGreen);
			DrawAABB(player);
			DrawAABB(obstacle);
		}

		private void DrawAABB(Box2D rect)
		{
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(rect.X, rect.Y);
			GL.Vertex2(rect.MaxX, rect.Y);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.X, rect.MaxY);
			GL.End();
		}

		private void DrawComplex(Box2D rect)
		{
			var xQuarter = rect.X + rect.SizeX * 0.25f;
			var x3Quarter = rect.X + rect.SizeX * 0.75f;
			var yThird = rect.Y + rect.SizeY * 0.33f;
			var y2Third = rect.Y + rect.SizeY * 0.66f;
			GL.Begin(PrimitiveType.Polygon);
			GL.Vertex2(rect.CenterX, rect.MaxY);
			GL.Vertex2(x3Quarter, y2Third);
			GL.Vertex2(rect.MaxX, rect.CenterY);
			GL.Vertex2(x3Quarter, yThird);
			GL.Vertex2(rect.MaxX, rect.Y);
			GL.Vertex2(rect.CenterX, yThird);
			GL.Vertex2(rect.X, rect.Y);
			GL.Vertex2(xQuarter, yThird);
			GL.Vertex2(rect.X, rect.CenterY);
			GL.Vertex2(xQuarter, y2Third);
			GL.End();
		}
	}
}