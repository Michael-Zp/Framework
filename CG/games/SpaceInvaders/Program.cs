using Zenseless.Application;
using Zenseless.Geometry;
using Zenseless.TimeTools;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace SpaceInvaders
{
	class Controller
	{
		private IImmutableBox2D windowBorders = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private Box2D player = new Box2D(0.0f, -1.0f, 0.1f, 0.05f);
		private List<Box2D> enemies = new List<Box2D>();
		private List<Box2D> bullets = new List<Box2D>();
		private PeriodicUpdate shootCoolDown = new PeriodicUpdate(0.1f);
		private Stopwatch timeSource = new Stopwatch();
		private float enemySpeed = 0.05f;
		private bool Lost;

		public Controller()
		{
			shootCoolDown.PeriodElapsed += (s, t) => shootCoolDown.Stop();
			CreateEnemies();
			timeSource.Start();
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			foreach (var enemy in enemies)
			{
				DrawEnemy(enemy);
			}
			foreach (var bullet in bullets)
			{
				DrawBullet(bullet);
			}
			DrawPlayer(player);
		}

		private void Update(float updatePeriod)
		{
			shootCoolDown.Update((float)timeSource.Elapsed.TotalSeconds);
			if (Lost)
			{
				return;
			}

			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];

			Update(updatePeriod, axisLeftRight, shoot);
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var controller = new Controller();
			window.Render += controller.Render;
			window.Update += controller.Update;
			window.Run();
		}

		private void Update(float timeDelta, float axisUpDown, bool shoot)
		{
			if (Lost) return;
			//remove outside bullet
			foreach (var bullet in bullets)
			{
				if (bullet.MinY > windowBorders.MaxX)
				{
					bullets.Remove(bullet);
					return;
				}
			}
			HandleCollisions();

			UpdatePlayer(timeDelta, axisUpDown, shoot);
			MoveEnemies(timeDelta);
			MoveBullets(timeDelta);

			if (0 == enemies.Count && 0 == bullets.Count)
			{
				//game is won -> start new, but faster
				CreateEnemies();
				enemySpeed += 0.05f;
			}
		}

		private static void DrawBullet(IImmutableBox2D o)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(Color.White);
			GL.Vertex2(o.MinX, o.MinY);
			GL.Vertex2(o.MaxX, o.MinY);
			GL.Vertex2(o.MaxX, o.MaxY);
			GL.Vertex2(o.MinX, o.MaxY);
			GL.End();
		}

		private static void DrawEnemy(IImmutableBox2D o)
		{
			GL.Begin(PrimitiveType.Triangles);
			GL.Color3(Color.White);
			GL.Vertex2(o.CenterX, o.CenterY);
			GL.Vertex2(o.MaxX, o.CenterY);
			GL.Vertex2(o.MaxX, o.MaxY);
			GL.Color3(Color.Violet);
			GL.Vertex2(o.CenterX, o.CenterY);
			GL.Vertex2(o.MinX, o.CenterY);
			GL.Vertex2(o.MinX, o.MinY);
			GL.End();
		}

		private static void DrawPlayer(IImmutableBox2D o)
		{
			GL.Begin(PrimitiveType.Triangles);
			GL.Color3(Color.GreenYellow);
			GL.Vertex2(o.MinX, o.MinY);
			GL.Vertex2(o.MaxX, o.MinY);
			GL.Vertex2(o.CenterX, o.MaxY);
			GL.End();
		}
		
		private void CreateEnemies()
		{
			//create enemies
			for (float y = 0.1f; y < 1.0f; y += 0.2f)
			{

				for (float x = -0.85f; x < 0.9f; x += 0.2f)
				{
					enemies.Add(new Box2D(x, y, 0.1f, 0.1f));
				}
			}
		}

		private void UpdatePlayer(float timeDelta, float axisUpDown, bool shoot)
		{
			player.MinX += timeDelta * axisUpDown;
			//limit player position [left, right]
			player.PushXRangeInside(windowBorders);

			if (shoot && !shootCoolDown.Enabled)
			{
				bullets.Add(new Box2D(player.MinX, player.MinY, 0.005f, 0.005f));
				bullets.Add(new Box2D(player.MaxX, player.MinY, 0.005f, 0.005f));
				shootCoolDown.Start((float)timeSource.Elapsed.TotalSeconds);
			}
		}

		private void HandleCollisions()
		{
			//intersections
			foreach (var enemy in enemies)
			{
				if (enemy.MinY < windowBorders.MinY)
				{
					//game lost
					Lost = true;
				}
				foreach (var bullet in bullets)
				{
					if (bullet.Intersects(enemy))
					{
						//delete bullet and enemy
						bullets.Remove(bullet);
						enemies.Remove(enemy);
						//need to return immediatly beause we change list
						return;
					}
				}
			}
		}

		private void MoveEnemies(float timeDelta)
		{
			foreach (Box2D enemy in enemies)
			{
				enemy.MinY -= enemySpeed * timeDelta;
			}
		}

		private void MoveBullets(float timeDelta)
		{
			foreach (var bullet in bullets)
			{
				bullet.MinY += timeDelta;
			}
		}
	}
}