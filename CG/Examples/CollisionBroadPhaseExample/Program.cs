using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();
		private List<Collider> colliders = new List<Collider>();
		private Box2D windowBorders = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private CollisionGrid collisionGrid;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run(60.0f, 60.0f);
		}

		private MyApplication()
		{
			gameWindow.WindowState = WindowState.Fullscreen;
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
			for (float x =  -0.8f; x < 0.8f; x += delta)
			{
				for (float y = -0.8f; y < 0.8f; y += delta)
				{
					colliders.Add(new Collider(x, y, size, size));
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

			//handle collisions
			if(Keyboard.GetState().IsKeyDown(Key.Space))
			{
				BruteForceCollision();
			}
			else
			{
				GridCollision();
			}
			Console.WriteLine(gameWindow.UpdateTime);
		}

		private void BruteForceCollision()
		{
			foreach (var collider in colliders)
			{
				foreach (var collider2 in colliders)
				{
					if (collider == collider2) continue;
					HandleNarrowPhaseCollision(collider, collider2);
				}
			}
		}
		private void GridCollision()
		{
			collisionGrid.Clear();
			foreach (var collider in colliders)
			{
				collisionGrid.Insert(collider);
			}
			for (int y = 0; y < collisionGrid.CellCountY; ++y)
			{
				for (int x = 0; x < collisionGrid.CellCountX; ++x)
				{
					var cell = collisionGrid[x, y];
					foreach (var collider1 in cell)
					{
						foreach (var collider2 in cell)
						{
							if (ReferenceEquals(collider1, collider2)) continue;
							var coll1 = collider1 as Collider;
							var coll2 = collider2 as Collider;
							HandleNarrowPhaseCollision(coll1, coll2);
						}
					}
				}
			}
		}

		private void HandleNarrowPhaseCollision(Collider collider1, Collider collider2)
		{
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
				var vel = collider1.Velocity;
				collider1.Velocity = -collider2.Velocity;
				collider2.Velocity = -vel;
				//collider2.Velocity = System.Numerics.Vector2.Zero;
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