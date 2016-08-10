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
			//registers another callback for drawing a frame, which is executed after the previous
			//this second one does the buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.RenderFrame += (s, a) => gameWindow.SwapBuffers();
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			//clear screen
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//draw a triangle
			GL.Begin(PrimitiveType.Triangles);
			//set color color is active as long as no other color is set
			GL.Color3(Color.White);
			GL.Vertex2(0.0, 0.0);
			GL.Vertex2(0.5, 0.0);
			GL.Vertex2(0.5, 0.5);
			GL.End();
		}
	}
}