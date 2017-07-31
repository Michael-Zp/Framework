using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	public class Program
	{
		[STAThread]
		private static void Main()
		{
			var wnd = new GameWindow();
			wnd.VSync = VSyncMode.Off;
			wnd.Resize += (s, a) => GL.Viewport(0, 0, wnd.Width, wnd.Height); //react on window size changes

			var x = -1f;
			GL.ClearColor(Color.CornflowerBlue);
			wnd.Visible = true;
			//main loop
			while (wnd.Exists)
			{
				if (x < 1) x += .01f; else x = -1;
				//clear screen - what happens without?
				GL.Clear(ClearBufferMask.ColorBufferBit);
				//draw a primitive
				GL.Begin(PrimitiveType.Quads);
				//color is active as long as no new color is set
				GL.Color3(Color.Cyan);
				GL.Vertex2(x + 0.0f, 0.0f);
				GL.Vertex2(x + 0.5f, 0.0f);
				GL.Color3(Color.White);
				GL.Vertex2(x + 0.5f, 0.5f);
				GL.Vertex2(x + 0.0f, 0.5f);
				GL.End();

				wnd.SwapBuffers();
				wnd.ProcessEvents(); //this call could destroy window, so do all stuff that needs the window (like gl) before
			}
		}
	}
}
