using DMS.OpenGL;
using System;

namespace Example
{
	class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			var canvas = new Canvas();
			var rasterizer = new Rasterizer(20, 20, canvas.Draw);
			app.Run(rasterizer);
		}
	}
}