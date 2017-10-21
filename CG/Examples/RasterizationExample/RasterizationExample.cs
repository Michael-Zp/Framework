using Zenseless.Application;
using Zenseless.Base;
using Zenseless.OpenGL;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Example
{
	public class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			var window = new ExampleWindow();
			var canvas = new Canvas();
			Bitmap screenshot = null; 
			var rasterizer = new Rasterizer(10, 10, canvas.Draw);
			window.Render += rasterizer.Render;
			window.Render += () => screenshot = FrameBuffer.ToBitmap();
			window.Run();
			if(!ReferenceEquals(null, screenshot))
			{
				var name = Path.ChangeExtension(PathTools.GetCurrentProcessPath(), ".png");
				screenshot.Save(name);
				Clipboard.SetImage(screenshot);

			}
		}
	}
}