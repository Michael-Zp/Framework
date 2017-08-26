using DMS.Application;
using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;
using DMS.HLGL;

namespace Example
{
	/// <summary>
	/// shows side scrolling setup by manipulating texture coordinates
	/// </summary>
	class MyVisual
	{
		private ITexture texBackground;
		private ITexture texPlayer;
		private Box2D texCoord = new Box2D(0, 0, 0.3f, 1);

		private MyVisual()
		{
			texPlayer = TextureLoader.FromBitmap(Resourcen.bird1);
			//two landscape resources are available in the Resourcen.resx file
			texBackground = TextureLoader.FromBitmap(Resourcen.forest);
			//set how texture coordinates outside of [0..1] are handled
			texBackground.WrapFunction = TextureWrapFunction.MirroredRepeat;
			//background clear color
			GL.ClearColor(Color.White);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
			GL.Enable(EnableCap.Blend); // enable transparency
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw background with changing texture coordinates
			DrawTexturedRect(new Box2D(-1, -1, 2, 2), texBackground, texCoord);

			DrawTexturedRect(new Box2D(-.25f, -.1f, .2f, .2f), texPlayer, new Box2D(0, 0, 1, 1)); // draw player
		}

		private void Update(float updatePeriod)
		{
			texCoord.MinX += updatePeriod * 0.1f; //scroll texture coordinates
		}

		private static void DrawTexturedRect(Box2D rect, ITexture tex, Box2D texCoords)
		{
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(texCoords.MinX, texCoords.MinY); GL.Vertex2(rect.MinX, rect.MinY);
			GL.TexCoord2(texCoords.MaxX, texCoords.MinY); GL.Vertex2(rect.MaxX, rect.MinY);
			GL.TexCoord2(texCoords.MaxX, texCoords.MaxY); GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.TexCoord2(texCoords.MinX, texCoords.MaxY); GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
			tex.Deactivate();
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual();
			window.Render += visual.Render;
			window.Update += visual.Update;
			window.Run();
		}
	}
}