using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using DMS.Application;
using DMS.HLGL;
using System.Numerics;

namespace Example
{
	class MyVisual
	{
		//private double timeSec = 0;
		private ITexture texBird;
		private Box2D bird = new Box2D(0, 0, .2f, .2f);
		private Stopwatch timeSource = new Stopwatch();
		private List<Vector2> wayPoints = new List<Vector2>();
		private List<Vector2> wayTangents;

		private MyVisual()
		{
			//set waypoints of enemy
			wayPoints.Add(new Vector2(-.5f, -.5f));
			wayPoints.Add(new Vector2(.5f, -.5f));
			wayPoints.Add(new Vector2(.5f, .5f));
			wayPoints.Add(new Vector2(-.5f, .5f));
			//wayPoints.Add(new Vector2(.6f, -.7f));
			//wayPoints.Add(new Vector2(.5f, .8f));
			//wayPoints.Add(new Vector2(-.5f, .4f));
			//wayPoints.Add(new Vector2(0, 0));
			wayTangents = CatmullRomSpline.FiniteDifferenceLoop(wayPoints);

			texBird = TextureLoader.FromBitmap(Resourcen.bird1);
			//background clear color
			GL.ClearColor(Color.White);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend); // for transparency in textures
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
			timeSource.Start();
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawTexturedRect(bird, texBird);
		}

		private void Update(float updatePeriod)
		{
			//timeSec += e.Time;
			//var seconds = (float)timeSec;
			var seconds = (float)timeSource.Elapsed.TotalSeconds;
			var activeSegment = CatmullRomSpline.FindSegment(seconds, wayPoints.Count);
			var pos = CatmullRomSpline.EvaluateSegment(wayPoints[activeSegment.Item1]
				, wayPoints[activeSegment.Item2]
				, wayTangents[activeSegment.Item1]
				, wayTangents[activeSegment.Item2]
				, seconds - (float)Math.Floor(seconds));
			
			bird = bird.MoveTo(pos);
		}

		private static void DrawTexturedRect(Box2D Rectangle, ITexture tex)
		{
			GL.Color3(Color.White);
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rectangle.MinX, Rectangle.MinY);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rectangle.MaxX, Rectangle.MinY);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rectangle.MaxX, Rectangle.MaxY);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rectangle.MinX, Rectangle.MaxY);
			GL.End();
			tex.Deactivate();
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual();
			window.Render += visual.Render;
			window.Update += visual.Update;
			window.Run();
		}
	}
}