﻿using Zenseless.Application;
using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.HLGL;

namespace Example
{
	/// <summary>
	/// Example that shows loading and using textures. 
	/// It loads 2 textures: one for the background and one for a space ship.
	/// </summary>
	class MyVisual
	{
		private ITexture texBackground;
		private ITexture texShip;

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual();
			window.Render += visual.Render;
			window.Run();
		}

		private MyVisual()
		{
			texShip = TextureLoader.FromBitmap(Resourcen.redship4);
			texBackground = TextureLoader.FromBitmap(Resourcen.water);
			//background clear color
			GL.ClearColor(Color.White);
			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend); // for transparency in textures we use blending
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color => white == no change to texture color
			GL.Color3(Color.White);
			//draw background
			DrawTexturedRect(new Box2D(-1, -1, 2, 2), texBackground);
			//draw ship
			DrawTexturedRect(new Box2D(-.25f, -.25f, .5f, .5f), texShip);
		}

		private static void DrawTexturedRect(IReadOnlyBox2D Rect, ITexture tex)
		{
			//the texture has to be enabled before use
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			//when using textures we have to set a texture coordinate for each vertex
			//by using the TexCoord command BEFORE the Vertex command
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rect.MinX, Rect.MinY);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rect.MaxX, Rect.MinY);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rect.MaxX, Rect.MaxY);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rect.MinX, Rect.MaxY);
			GL.End();
			//the texture is disabled, so no other draw calls use this texture
			tex.Deactivate();
		}
	}
}