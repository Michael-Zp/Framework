﻿using Zenseless.Application;
using Zenseless.Geometry;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Example
{
	class MyVisual
	{
		private MyVisual()
		{
			//background clear color
			GL.ClearColor(1, 1, 1, 1);
			//setup blending equation Color = Color_new · alpha + Color_before · (1 - alpha)
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.Enable(EnableCap.Blend);
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			var rect = new Box2D(-.75f, -.75f, .5f, .5f);
			DrawRect(rect, new Color4(.5f, .7f, .1f, 1));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.7f, .5f, .9f, .5f));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.7f, .5f, .9f, .5f));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.5f, .7f, 1, .5f));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.5f, .7f, 1, .5f));
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual();
			window.Render += visual.Render;
			window.Run();
		}

		private void DrawRect(IImmutableBox2D rectangle, Color4 color)
		{
			GL.Color4(color);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(rectangle.MinX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.Vertex2(rectangle.MinX, rectangle.MaxY);
			GL.End();
		}

		public void Update()
		{
			Render();
		}
	}
}