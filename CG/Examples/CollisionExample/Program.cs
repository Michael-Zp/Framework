using Framework;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();
		private List<Box2D> enemies = new List<Box2D>();
		private Box2D player = new Box2D(0.0f, -1.0f, 0.1f, 0.05f);
		private Stopwatch timeSource = new Stopwatch();
		private PeriodicUpdate periodicEnemyCreate = new PeriodicUpdate(1.0f);

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
			//setup callback for creating enemies
			periodicEnemyCreate.OnPeriodElapsed += PeriodicEnemyCreate_OnPeriodElapsed;
			//start the game time
			timeSource.Start();
			periodicEnemyCreate.Start((float)timeSource.Elapsed.TotalSeconds);
		}

		private void PeriodicEnemyCreate_OnPeriodElapsed(PeriodicUpdate sender, float absoluteTime)
		{
			enemies.Add(new Box2D(0, 1, 0.1f, 0.1f));
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			//enemies.Add(new Box2D(0, 1, 0.1f, 0.1f));
			periodicEnemyCreate.Update((float)timeSource.Elapsed.TotalSeconds);
			foreach (var enemy in enemies)
			{
				enemy.Y -= 0.005f;
			}
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			//clear screen - what happens without?
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.White);
			//draw the player 
			DrawBox(player);

			GL.Color3(Color.Red);
			foreach (var enemy in enemies)
			{
				DrawBox(enemy);
			}
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