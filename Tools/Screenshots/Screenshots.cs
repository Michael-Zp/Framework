using Zenseless.OpenGL;
using OpenTK;
using System.Drawing;

namespace Screenshots
{
	class Screenshots
	{
		static void Main(string[] args)
		{
			var gameWindow = new GameWindow();
			var provider = new ExampleProvider();
			gameWindow.Visible = true;
			//while (gameWindow.Exists)
			{
				foreach(var example in provider.Examples)
				{
					example.Update();
					var name = example.GetType().Assembly.GetName().Name;
					var bitmap = ReadBack.FrameBuffer();
					var b2 = new Bitmap(bitmap);
					if(b2 == bitmap)
					{

					}
					//bitmap.Save(name + ".png");

				}
				//var bitmap = ReadBack.FrameBuffer();
				//bitmap.Save();
				gameWindow.SwapBuffers();
				gameWindow.ProcessEvents();
			}
			provider.Dispose();
			gameWindow.Dispose();
		}
	}
}
