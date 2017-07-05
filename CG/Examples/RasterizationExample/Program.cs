using DMS.Application;
using DMS.OpenGL;
using System;
using System.Windows.Forms;

namespace Example
{
	public class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			var canvas = new Canvas();
			var rasterizer = new Rasterizer(10, 10, canvas.Draw);
			app.Render += rasterizer.Render;
			//app.Render += () => Screenshot();
			app.Run();
		}

		private static void Screenshot()
		{
			var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			var bmp = ReadBack.FrameBuffer();
			Clipboard.SetImage(bmp);
			bmp.Save(@"d:\" + name + ".png");
		}
	}
}