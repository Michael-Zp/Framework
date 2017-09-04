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
			window.Render += () => screenshot = ReadBack.FrameBuffer();
			window.Run();
			if(!ReferenceEquals(null, screenshot))
			{
				var saveDirectory = PathTools.GetNewAssemblyOutputDataPath();
				Directory.CreateDirectory(saveDirectory);
				screenshot.Save(saveDirectory + "output.png");
				Clipboard.SetImage(screenshot);

			}
		}
	}
}