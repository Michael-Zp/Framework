using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private Texture texBird;
		private Vector2 rotCenter = new Vector2(-.9f, 0);
		private List<Box2D> birds = new List<Box2D>();

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run(60);
		}

		private MyApplication()
		{
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
			GL.ClearColor(Color.DarkGreen);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//generate birds
			for(float delta = .1f; delta < .5f; delta += .1f)
			{
				birds.Add(Box2dExtensions.CreateFromCenterSize(rotCenter.X - delta, rotCenter.Y - delta, .1f, .1f));
				birds.Add(Box2dExtensions.CreateFromCenterSize(rotCenter.X - delta, rotCenter.Y + delta, .1f, .1f));
				birds.Add(Box2dExtensions.CreateFromCenterSize(rotCenter.X + delta, rotCenter.Y - delta, .1f, .1f));
				birds.Add(Box2dExtensions.CreateFromCenterSize(rotCenter.X + delta, rotCenter.Y + delta, .1f, .1f));
			}
		}


		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			rotCenter.X += 0.003f;
			var R = Transform2D.CreateRotationAround(rotCenter.X, rotCenter.Y, (float)gameWindow.TargetUpdatePeriod * 4);
			foreach (var bird in birds)
			{
				bird.TransformCenter(R);
			}
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Enable(EnableCap.Blend); // for transparency in textures
			foreach (var bird in birds)
			{
				DrawTexturedRect(bird, texBird);
			}
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