﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class Program
	{
		[STAThread]
		private static void Main()
		{
			var fb = new Framebuffer(512, 512);
			GL.ClearColor(Color.CornflowerBlue);
			//wnd.Visible = true;
			while (fb.NextFrame())
			{
				//clear screen - what happens without?
				GL.Clear(ClearBufferMask.ColorBufferBit);
				//draw a quad
				GL.Begin(PrimitiveType.Quads);
					//color is active as long as no new color is set
					GL.Color3(Color.Cyan);
					GL.Vertex2(0.0f, 0.0f); //draw first quad corner
					GL.Vertex2(0.5f, 0.0f);
					GL.Vertex2(0.5f, 0.5f);
					GL.Vertex2(0.0f, 0.5f);
				GL.End();
				//wnd.SwapBuffers(); //double buffering
				//wnd.ProcessEvents(); //handle all events that are sent to the window (user inputs, operating system stuff); this call could destroy window, so check immediatily after this call if window still exists, otherwise gl calls will fail.
			}
		}
	}
}