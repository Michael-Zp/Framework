using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();
		private List<Collider> colliders = new List<Collider>();
		private Box2D windowBorders = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private CollisionGrid collisionGrid;
		private Stopwatch stopWatch = new Stopwatch();
		private double smoothedBenchmark = 0;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run(60.0f, 60.0f);
		}

		private MyApplication()
		{
			//gameWindow.WindowState = WindowState.Fullscreen;
			GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			SetupColliders();
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;

			gameWindow.KeyDown += GameWindow_KeyDown;
		}

		private void SetupColliders()
		{
			float delta = 0.05f;
			float space = 0.02f;
			float distance2 = space / 2;
			float size = delta - space;
			int i = 0;
			for (float x =  -0.8f; x < 0.8f; x += delta)
			{
				for (float y = -0.8f; y < 0.8f; y += delta)
				{
					var collider = new Collider(x, y, size, size);
					var vel = Collider.RndVelocity();
					if(0.1f < vel.Length())
					{
						collider.Velocity = vel;
					}
					colliders.Add(collider);
					++i;
				}
			}
			float scale = 2f;
			collisionGrid = new CollisionGrid(windowBorders, colliders.First().Box.SizeX * scale, colliders.First().Box.SizeY * scale);
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
			//movement
			foreach (var collider in colliders)
			{
				collider.Box.X += collider.Velocity.X * updatePeriod;
				collider.Box.Y += collider.Velocity.Y * updatePeriod;
				if (!collider.Box.Inside(windowBorders))
				{
					collider.Box.PushXRangeInside(windowBorders);
					collider.Box.PushYRangeInside(windowBorders);
					collider.Velocity = -collider.Velocity;
				}
			}

			stopWatch.Restart();
			//handle collisions
			if(Keyboard.GetState().IsKeyDown(Key.Space))
			{
				BruteForceCollision();
			}
			else
			{
				GridCollision();
			}
			stopWatch.Stop();
			smoothedBenchmark = Geometry.MathHelper.Lerp(stopWatch.Elapsed.TotalMilliseconds, smoothedBenchmark, 0.9);
			Console.WriteLine(smoothedBenchmark);
		}

		private void BruteForceCollision()
		{
			for (int i = 0; i < colliders.Count; ++i)
			{
				for (int j = i + 1; j < colliders.Count; ++j)
				{
					HandleNarrowPhaseCollisionOrdering(colliders[i], colliders[j]);
				}
			}
		}

		private void GridCollision()
		{
			collisionGrid.FindAllCollisions(colliders, (c1, c2) => HandleNarrowPhaseCollisionOrdering(c1 as Collider, c2 as Collider));
		}

		private void HandleNarrowPhaseCollisionOrdering(Collider collider1, Collider collider2)
		{
			if (System.Numerics.Vector2.Zero == collider1.Velocity)
			{
				if (System.Numerics.Vector2.Zero != collider2.Velocity)
				{
					HandleNarrowPhaseCollision(collider2, collider1);
				}
			}
			else
			{
				HandleNarrowPhaseCollision(collider1, collider2);
			}
		}
		private void HandleNarrowPhaseCollision(Collider collider1, Collider collider2)
		{
			if (System.Numerics.Vector2.Zero == collider1.Velocity)
			{
				if (System.Numerics.Vector2.Zero == collider2.Velocity)
				{
					return;
				}
				HandleNarrowPhaseCollision(collider2, collider1);
			}
			var box1 = collider1.Box;
			var box2 = collider2.Box;
			if (box1.Intersects(box2))
			{
				//float updatePeriod = (float)gameWindow.TargetUpdatePeriod;
				//collider1.Box.X -= collider1.Velocity.X * updatePeriod;
				//collider1.Box.Y -= collider1.Velocity.Y * updatePeriod;
				//collider2.Box.X -= collider2.Velocity.X * updatePeriod;
				//collider2.Box.Y -= collider2.Velocity.Y * updatePeriod;
				box1.UndoOverlap(box2);
				collider1.Velocity = Collider.RndVelocity();
			}
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			foreach (var collider in colliders)
			{
				GL.Color3(collider.Color);
				DrawBox(collider.Box);
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