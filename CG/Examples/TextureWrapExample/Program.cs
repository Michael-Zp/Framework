using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Example
{
	/// <summary>
	/// shows side scrolling setup by manipulating texture coordinates
	/// </summary>
	class MyApplication
	{
		private GameWindow gameWindow;
		private Texture texBackground;
		private Box2D texCoord = new Box2D(-1, -1, 3, 3);

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			//setup
			gameWindow = new GameWindow(512, 512);
			gameWindow.KeyDown += (s, arg) => gameWindow.Close();
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			texBackground = TextureLoader.FromBitmap(Resourcen.mountains);
			//background clear color
			GL.ClearColor(Color.White);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);

			//draw with different wrap modes

			//set how texture coordinates outside of [0..1] are handled
			texBackground.WrapMode(TextureWrapMode.Repeat);
			DrawTexturedRect(new Box2D(-1, -1, 1, 1), texBackground, texCoord);
			texBackground.WrapMode(TextureWrapMode.Clamp);
			DrawTexturedRect(new Box2D(0, -1, 1, 1), texBackground, texCoord);
			texBackground.WrapMode(TextureWrapMode.ClampToBorder);
			DrawTexturedRect(new Box2D(-1, 0, 1, 1), texBackground, texCoord);
			texBackground.WrapMode(TextureWrapMode.MirroredRepeat);
			DrawTexturedRect(new Box2D(0, 0, 1, 1), texBackground, texCoord);
		}

		private static void DrawTexturedRect(Box2D rect, Texture tex, Box2D texCoords)
		{
			tex.BeginUse();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(texCoords.X, texCoords.Y); GL.Vertex2(rect.X, rect.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.Y); GL.Vertex2(rect.MaxX, rect.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.MaxY); GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.TexCoord2(texCoords.X, texCoords.MaxY); GL.Vertex2(rect.X, rect.MaxY);
			GL.End();
			tex.EndUse();
		}
	}
}