using DMS.Geometry;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Example
{
	class MyWindow : IWindow
	{
		private Box2D obstacle = new Box2D(-0.2f, 1, 0.4f, 0.2f);
		private Box2D player = new Box2D(0.0f, -0.95f, 0.2f, 0.2f);

		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}

		public void Update(float updatePeriod)
		{
			//player movement
			if(Keyboard.GetState()[Key.Left])
			{
				player.X -= updatePeriod;
			}
			else if (Keyboard.GetState()[Key.Right])
			{
				player.X += updatePeriod;
			}
			//todo: let the player also move up down
			//todo:Limit player movements to window

			//no intersection -> move obstacle
			if (!obstacle.Intersects(player))
			{
				obstacle.Y -= 0.5f * updatePeriod;
			}

			if(obstacle.MaxY < -1)
			{
				obstacle.Y = 1;
			}
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.CornflowerBlue);
			DrawComplex(player);
			DrawComplex(obstacle);

			GL.LineWidth(3.0f);
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