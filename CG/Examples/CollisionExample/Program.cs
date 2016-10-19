using Geometry;
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
		private Box2D obstacle = new Box2D(0, 1, 0.1f, 0.1f);
		private Box2D player = new Box2D(0.0f, -0.95f, 0.1f, 0.05f);
		private Box2D windowBorders = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);

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

			obstacle.Y -= 0.5f * updatePeriod;

			//player movement
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			player.X += updatePeriod * axisLeftRight;
			//todo: limit player position [left, right]

			//check if obstacle intersects player
			if(obstacle.Intersects(player))
			{
				//stop updates
				gameWindow.UpdateFrame -= GameWindow_UpdateFrame;
			}
			//check if obstacle has reached lower border
			if (obstacle.Y < windowBorders.Y)
			{
				//stop updates
				gameWindow.UpdateFrame -= GameWindow_UpdateFrame;
			}
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			//clear screen - what happens without?
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.White);
			DrawBox(player);

			GL.Color3(Color.Red);
			DrawBox(obstacle);
		}
		
		private void DrawBox(Box2D rect)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(rect.X, rect.Y);
			GL.Vertex2(rect.MaxX, rect.Y);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.X, rect.MaxY);
			GL.End();
		}
	}
}