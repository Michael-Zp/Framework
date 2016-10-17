using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();
		private float y = 1.0f;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			//clear screen - what happens without?
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//draw a primitive
			GL.Begin(PrimitiveType.Quads);
			//set color color is active as long as no other color is set
			GL.Color3(Color.White);
			GL.Vertex2(0.0f, 0.0f);
			GL.Vertex2(0.5f, 0.0f);
			GL.Vertex2(0.5f, 0.5f);
			GL.Vertex2(0.0f, 0.5f);
			GL.End();

			y -= 0.005f;
			DrawEnemy();

			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.SwapBuffers();
		}
		
		private void DrawEnemy()
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(Color.Red);
			GL.Vertex2(0.0f, y);
			GL.Color3(Color.White);
			GL.Vertex2(0.25f, y);
			GL.Color3(Color.Green);
			GL.Vertex2(0.25f, y + 0.25f);
			GL.Vertex2(0.0f, y + 0.25f);
			GL.End();
		}
	}
}