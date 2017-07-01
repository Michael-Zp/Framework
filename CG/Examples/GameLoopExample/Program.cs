using DMS.Application;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var wnd = new GameWindow();
			var resMng = new ResourceManager();
			Resources.LoadResources(resMng);
			var visual = new MainVisual();
			wnd.Resize += (s, a) => GL.Viewport(0 , 0, wnd.Width, wnd.Height);
			wnd.ConnectEvents(visual.Camera);
			wnd.VSync = VSyncMode.Off;

			//var x = -1f;
			//GL.ClearColor(.5f, .2f, 0, 1);
			wnd.Visible = true;
			//main loop
			while (wnd.Exists)
			{
				//if (x < 1) x += .01f; else x = -1;
				//GL.Clear(ClearBufferMask.ColorBufferBit);
				//GL.Begin(PrimitiveType.Triangles);
				//GL.Vertex2(x, 0);
				//GL.Vertex2(x + 1, 0);
				//GL.Vertex2(x, 1);
				//GL.End();
				visual.Render();
				wnd.Context.SwapBuffers();
				wnd.ProcessEvents(); //this call could destroy window, so do all stuff that needs the window (like gl) before
			}
		}
	}
}
