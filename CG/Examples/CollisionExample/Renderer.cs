using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using Zenseless.Base;
using Zenseless.Geometry;

namespace Example
{
	public class Renderer
	{
		public Renderer(ITime time)
		{
			this.time = time;
			ShapeBuilder.Circle((float x, float y) => circle.Add(new Vector2(x, y)), 0f, 0f, .5f, 20); //create circle
			var rndGenerator = new Random(12);
			Func<float> Rnd = () => .9f + (float)rndGenerator.NextDouble() * .1f; //random number in range [0.9, 1]
			for(int i = 0; i < circle.Count; ++i)
			{
				circle[i] = circle[i] * Rnd(); //scale circle with random value;
			}
			GL.LineWidth(3.0f);
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Ortho(-aspect, aspect, -1, 1, 0, 1);
		}

		public void DrawShape(IReadOnlyBox2D box)
		{
			GL.Color3(Color.LightSlateGray);
			DrawObstacle(box);
			GL.Color3(Color.DeepSkyBlue);
			DrawBoxOutline(box);
		}

		public void Resize(int width, int height)
		{
			aspect = width / (float)height;
		}

		private List<Vector2> circle = new List<Vector2>();
		private float aspect = 1f;
		private ITime time;

		private static void DrawBoxOutline(IReadOnlyBox2D rect)
		{
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(rect.MinX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
		}

		private void DrawObstacle(IReadOnlyBox2D rect)
		{
			GL.PushMatrix();
				GL.Translate(rect.MinX, rect.MinY, 0f);
				GL.Scale(rect.SizeX, rect.SizeY, 0f);
				GL.Translate(.5, .5, 0);
				GL.Rotate(time.AbsoluteTime * 100, 0, 0, 1);
				GL.Begin(PrimitiveType.Polygon);
					foreach (var p in circle)
					{
						GL.Vertex2(p);
					}
				GL.End();
			GL.PopMatrix();
		}
	}
}
