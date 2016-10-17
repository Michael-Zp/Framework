using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();

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
			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.SwapBuffers();
		}
	}
}