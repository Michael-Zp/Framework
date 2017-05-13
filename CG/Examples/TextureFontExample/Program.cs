using DMS.Application;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class MyVisual : IWindow
	{
		private TextureFont font;

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyVisual());
		}

		private MyVisual()
		{
			//load font
			font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Blood_Bath_2), 10, 32, .8f, 1, .7f);
			//background clear color
			GL.ClearColor(Color.Black);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend); // for transparency in textures
			GL.Color3(Color.White); //color is multiplied with texture color white == no change
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			font.Print(-.9f, -.125f, 0, .25f, "SUPER GEIL"); //print string
		}

		public void Update(float updatePeriod)
		{
		}
	}
}