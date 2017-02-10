using DMS.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private TextureFont font;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			//setup
			gameWindow = new GameWindow(700, 700);
			gameWindow.KeyDown += (s, arg) => gameWindow.Close();
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			//load font
			font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Blood_Bath_2), 10, 32, .8f, 1, .7f);
			//background clear color
			GL.ClearColor(Color.Black);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);

			GL.Enable(EnableCap.Blend); // for transparency in textures
			//print string
			font.Print(-.9f, -.125f, 0, .25f, "SUPER GEIL");
			GL.Disable(EnableCap.Blend); // for transparency in textures
		}
	}
}