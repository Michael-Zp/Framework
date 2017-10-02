using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.Application;

namespace Example
{
	/// <summary>
	/// Shows occlusion queries in action
	/// </summary>
	class MyVisual
	{
		private MyVisual()
		{
			queryA = new QueryObject();
			queryB = new QueryObject();
			//for query to work
			GL.Enable(EnableCap.DepthTest);
		}

		float moveDelta = 0.01f;
		private Box2D boxA = new Box2D(-.2f, -.2f, .4f, .4f);
		private Box2D boxB = new Box2D(-.5f, -.1f, .2f, .2f);
		private QueryObject queryA, queryB;

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Color3(Color.White);
			queryA.Activate(OpenTK.Graphics.OpenGL4.QueryTarget.SamplesPassed);
			DrawBox(boxA, 0.0f);
			queryA.Deactivate();

			GL.Color3(Color.Red);
			queryB.Activate(OpenTK.Graphics.OpenGL4.QueryTarget.SamplesPassed);
			DrawBox(boxB, 0.5f);
			queryB.Deactivate();

			Console.WriteLine(queryA.Result);
			Console.WriteLine(queryB.Result);

		}

		private void Update(float updatePeriod)
		{
			moveDelta = (boxB.CenterX > 0.5f) ? -Math.Abs(moveDelta) : (boxB.CenterX < -0.5f) ? Math.Abs(moveDelta) : moveDelta;

			boxB.MinX += moveDelta;
		}

		private static void DrawBox(Box2D rect, float depth)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex3(rect.MinX, rect.MinY, depth);
			GL.Vertex3(rect.MaxX, rect.MinY, depth);
			GL.Vertex3(rect.MaxX, rect.MaxY, depth);
			GL.Vertex3(rect.MinX, rect.MaxY, depth);
			GL.End();
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			var visual = new MyVisual();
			app.Update += visual.Update;
			app.Render += visual.Render;
			app.Run();
		}
	}
}