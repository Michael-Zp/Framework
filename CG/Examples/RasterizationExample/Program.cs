using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private Rasterizer rasterizer;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our draw callback
			app.rasterizer.Run();
		}

		private MyApplication()
		{
			rasterizer = new Rasterizer(20, 20, Draw);
		}

		private void Draw()
		{
			GL.Color3(0.2, 0.4, 1);
			GL.Begin(PrimitiveType.Lines);
				GL.Vertex2(-1, -1);
				GL.Vertex2(1, 1);
			GL.End();
		}
	}
}