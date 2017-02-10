using DMSOpenGL;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
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
		private Texture texPlayer;
		private Box2D texCoord = new Box2D(0, 0, 0.3f, 1);

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
			//callback for updating http://gameprogrammingpatterns.com/game-loop.html
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			texPlayer = TextureLoader.FromBitmap(Resourcen.bird1);
			//two landscape resources are available in the Resourcen.resx file
			texBackground = TextureLoader.FromBitmap(Resourcen.forest);
			//set how texture coordinates outside of [0..1] are handled
			texBackground.WrapMode(TextureWrapMode.Repeat);
			//background clear color
			GL.ClearColor(Color.White);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			//this callback is guaranteed to be called as often as you specified with the gameWindow.Run(updateFrequency) call
			//so you have a fixed number of calls per second e.x.: 60 times per second
			texCoord.X += 0.001f;
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw background
			DrawTexturedRect(new Box2D(-1, -1, 2, 2), texBackground, texCoord);

			GL.Enable(EnableCap.Blend); // for transparency in textures
										//draw player
			DrawTexturedRect(new Box2D(-.25f, -.1f, .2f, .2f), texPlayer, new Box2D(0, 0, 1, 1));
			GL.Disable(EnableCap.Blend); // for transparency in textures
		}

		private static void DrawTexturedRect(Box2D rect, Texture tex, Box2D texCoords)
		{
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(texCoords.X, texCoords.Y); GL.Vertex2(rect.X, rect.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.Y); GL.Vertex2(rect.MaxX, rect.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.MaxY); GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.TexCoord2(texCoords.X, texCoords.MaxY); GL.Vertex2(rect.X, rect.MaxY);
			GL.End();
			tex.Deactivate();
		}
	}
}