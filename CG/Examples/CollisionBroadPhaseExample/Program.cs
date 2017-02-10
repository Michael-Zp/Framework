using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace Example
{
	class MyApplication
	{
		private OpenTK.GameWindow gameWindow = new OpenTK.GameWindow();
		private List<Collider> colliders = new List<Collider>();
		private Box2D windowBorders = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private CollisionGrid collisionGrid;
		private Stopwatch stopWatch = new Stopwatch();
		private double smoothedBenchmark = 0;
		private static Random rnd = new Random(12);

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run(60.0f, 60.0f);
		}

		private MyApplication()
		{
			//gameWindow.WindowState = OpenTK.WindowState.Fullscreen;
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
			float delta = 0.03f;
			float space = 0.01f;
			float distance2 = space / 2;
			float size = delta - space;
			int i = 0;
			for (float x =  -0.9f; x < 0.9f; x += delta)
			{
				for (float y = -0.9f; y < 0.9f; y += delta)
				{
					var collider = new Collider(x, y, size, size);
					collider.Velocity = RndVelocity();
					colliders.Add(collider);
					++i;
				}
			}
			float scale = 2f;
			collisionGrid = new CollisionGrid(windowBorders, size * scale, size * scale);
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key)
			{
				gameWindow.Exit();
			}
		}

		private void GameWindow_UpdateFrame(object sender, OpenTK.FrameEventArgs e)
		{
			float updatePeriod = (float)gameWindow.TargetUpdatePeriod;
			//movement
			foreach (var collider in colliders)
			{
				collider.SaveBox();
				collider.Box.X += collider.Velocity.X * updatePeriod;
				collider.Box.Y += collider.Velocity.Y * updatePeriod;
				if (!collider.Box.Inside(windowBorders))
				{
					collider.Box.PushXRangeInside(windowBorders);
					collider.Box.PushYRangeInside(windowBorders);
					collider.Velocity = -collider.Velocity;
				}
			}

			stopWatch.Restart(); //measure time spend on collision detection
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

			var inertness = MathHelper.Clamp(gameWindow.UpdatePeriod, 0.001, 1.0);
			smoothedBenchmark = MathHelper.Lerp(smoothedBenchmark, stopWatch.Elapsed.TotalMilliseconds, inertness);
			Console.WriteLine(smoothedBenchmark.ToString());
		}

		private void BruteForceCollision()
		{
			for (int i = 0; i < colliders.Count; ++i)
			{
				for (int j = i + 1; j < colliders.Count; ++j)
				{
					 HandleNarrowPhaseCollision(colliders[i], colliders[j]);
				}
			}
		}

		private void GridCollision()
		{
			collisionGrid.FindAllCollisions(colliders, (c1, c2) => HandleNarrowPhaseCollision(c1 as Collider, c2 as Collider));
		}

		private void HandleNarrowPhaseCollision(Collider collider1, Collider collider2)
		{
			var box1 = collider1.Box;
			var box2 = collider2.Box;
			if (box1.Intersects(box2))
			{
				//undo movement
				collider1.RestoreSavedBox();
				collider2.RestoreSavedBox();
				////set random velocity
				collider1.Velocity = RndVelocity();
				collider2.Velocity = RndVelocity();
			}
		}

		private void GameWindow_RenderFrame(object sender, OpenTK.FrameEventArgs e)
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

		private Vector2 RndVelocity()
		{
			var rndData = new byte[2];
			rnd.NextBytes(rndData);
			var velocity = new Vector2(rndData[0] - 128, rndData[1] - 128);
			velocity *= 0.0005f;
			return velocity;
		}
	}
}