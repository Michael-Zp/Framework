using Zenseless.Application;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Example
{
	class Controller
	{
		private Box2D obstacle = new Box2D(-0.2f, 1, 0.4f, 0.2f);
		private Box2D player = new Box2D(0.0f, -0.95f, 0.2f, 0.2f);

		private void Update(float updatePeriod)
		{
			//player movement
			if(Keyboard.GetState()[Key.Left])
			{
				player.MinX -= updatePeriod;
			}
			else if (Keyboard.GetState()[Key.Right])
			{
				player.MinX += updatePeriod;
			}
			//todo student: let the player also move up and down
			//todo student:Limit player movements to window

			//no intersection -> move obstacle
			if (!obstacle.Intersects(player))
			{
				obstacle.MinY -= 0.5f * updatePeriod;
			}

			if(obstacle.MaxY < -1)
			{
				obstacle.MinY = 1;
			}
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.CornflowerBlue);
			DrawComplex(player);
			DrawComplex(obstacle);

			GL.LineWidth(2.0f);
			GL.Color3(Color.YellowGreen);
			DrawBoxOutline(player);
			DrawBoxOutline(obstacle);
		}

		private void DrawBoxOutline(Box2D rect)
		{
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(rect.MinX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
		}

		private void DrawComplex(Box2D rect)
		{
			var xQuarter = rect.MinX + rect.SizeX * 0.25f;
			var x3Quarter = rect.MinX + rect.SizeX * 0.75f;
			var yThird = rect.MinY + rect.SizeY * 0.33f;
			var y2Third = rect.MinY + rect.SizeY * 0.66f;
			GL.Begin(PrimitiveType.Polygon);
			GL.Vertex2(rect.CenterX, rect.MaxY);
			GL.Vertex2(x3Quarter, y2Third);
			GL.Vertex2(rect.MaxX, rect.CenterY);
			GL.Vertex2(x3Quarter, yThird);
			GL.Vertex2(rect.MaxX, rect.MinY);
			GL.Vertex2(rect.CenterX, yThird);
			GL.Vertex2(rect.MinX, rect.MinY);
			GL.Vertex2(xQuarter, yThird);
			GL.Vertex2(rect.MinX, rect.CenterY);
			GL.Vertex2(xQuarter, y2Third);
			GL.End();
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
	}
}