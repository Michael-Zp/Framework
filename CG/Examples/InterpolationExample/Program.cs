using Framework;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		//private double timeSec = 0;
		private GameWindow gameWindow;
		private Texture texBird;
		private Box2D bird = new Box2D(0, 0, .2f, .2f);
		private Stopwatch timeSource = new Stopwatch();
		private List<Vector2> wayPoints = new List<Vector2>();
		private List<Vector2> wayTangents;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run();
		}

		private MyApplication()
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
			//setup
			gameWindow = new GameWindow(700, 700);
			gameWindow.VSync = VSyncMode.On;
			gameWindow.KeyDown += (s, arg) => gameWindow.Close();
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			texBird = TextureLoader.FromBitmap(Resourcen.bird1);
			//background clear color
			GL.ClearColor(Color.White);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			timeSource.Start();
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
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

			bird.CenterX = pos.X;
			bird.CenterY = pos.Y;
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Enable(EnableCap.Blend); // for transparency in textures
			//draw enemy
			DrawTexturedRect(bird, texBird);
			GL.Disable(EnableCap.Blend); // for transparency in textures
		}

		private void DrawTexturedRect(Box2D Rectangle, Texture tex)
		{
			GL.Color3(Color.White);
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rectangle.X, Rectangle.Y);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rectangle.MaxX, Rectangle.Y);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rectangle.MaxX, Rectangle.MaxY);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rectangle.X, Rectangle.MaxY);
			GL.End();
			tex.Deactivate();
		}
	}
}