using DMS.Application;
using System;

namespace Example
{
	class MyApplication
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var canvas = new Canvas();
			var rasterizer = new Rasterizer(20, 20, canvas.Draw);
			app.Render += rasterizer.Render;
			app.Run();
		}
	}
}