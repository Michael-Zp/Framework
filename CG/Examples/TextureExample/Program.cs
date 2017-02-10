using DMSOpenGL;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	/// <summary>
	/// Example that shows loading and using textures. 
	/// It loads 2 textures: one for the background and one for a space ship.
	/// </summary>
	class MyApplication
	{
		private GameWindow gameWindow;
		private Texture texBackground;
		private Texture texShip;

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
			texShip = TextureLoader.FromBitmap(Resourcen.redship4);
			texBackground = TextureLoader.FromBitmap(Resourcen.water);
			//background clear color
			GL.ClearColor(Color.White);
			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color => white == no change to texture color
			GL.Color3(Color.White);
			//draw background
			DrawTexturedRect(new Box2D(-1, -1, 2, 2), texBackground);
			// for transparency in textures we use blending
			GL.Enable(EnableCap.Blend);
			//draw ship
			DrawTexturedRect(new Box2D(-.25f, -.25f, .5f, .5f), texShip);
			// for transparency in textures we use blending
			GL.Disable(EnableCap.Blend);
		}

		private static void DrawTexturedRect(Box2D Rect, Texture tex)
		{
			//the texture has to be enabled before use
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
				//when using textures we have to set a texture coordinate for each vertex
				//by using the TexCoord command BEFORE the Vertex command
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rect.X, Rect.Y);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rect.MaxX, Rect.Y);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rect.MaxX, Rect.MaxY);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rect.X, Rect.MaxY);
			GL.End();
			//the texture is disabled, so no other draw calls use this texture
			tex.Deactivate();
		}
	}
}