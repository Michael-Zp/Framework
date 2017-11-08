﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	public class Renderer
	{
		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void Resize(int width, int height)
		{
			var size = Math.Min(width, height) * .01f; //keep points roughly the same size for different window sizes
			GL.PointSize(size);
		}

		/// <summary>
		/// Very inefficient function if you draw multiple points, but easy to understand
		/// </summary>
		/// <param name="location"></param>
		/// <param name="color"></param>
		public void DrawPoint(Vector2 location, Color color)
		{
			GL.Color3(color);
			GL.Begin(PrimitiveType.Points);
			GL.Vertex2(location.X, location.Y);
			GL.End();
		}

		public void DrawPoint(Vector2 location, float age)
		{
			DrawPoint(location, ConvertAgeToColor(age));
		}

		private static Color ConvertAgeToColor(float age)
		{
			var brightness = (int)((1f - MathHelper.Clamp(age, 0f, 1f)) * 255);
			return Color.FromArgb(brightness, brightness, brightness);
		}
	}
}
