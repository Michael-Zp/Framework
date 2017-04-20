using DMS.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class MyWindow : IWindow
	{
		private TextureFont font;

		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}

		private MyWindow()
		{
			//load font
			font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Blood_Bath_2), 10, 32, .8f, 1, .7f);
			//background clear color
			GL.ClearColor(Color.Black);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);

			GL.Enable(EnableCap.Blend); // for transparency in textures
										//print string
			font.Print(-.9f, -.125f, 0, .25f, "SUPER GEIL");
			GL.Disable(EnableCap.Blend); // for transparency in textures
		}

		public void Update(float updatePeriod)
		{
		}
	}
}