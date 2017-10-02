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
	/// Compares texture minification filter methods
	/// </summary>
	class MyVisual
	{
		private ITexture texBackgroundLinear;
		private ITexture texBackgroundMipmap;
		private Box2D texCoord = new Box2D(0, 0, 1, 1);
		private float scaleFactor = 1f;

		private MyVisual()
		{
			texBackgroundLinear = TextureLoader.FromBitmap(Resourcen.mountains);
			texBackgroundLinear.Filter = TextureFilterMode.Linear; //filter by taking the nearest texel's color as a pixels color
			texBackgroundLinear.WrapFunction = TextureWrapFunction.Repeat;
			texBackgroundMipmap = TextureLoader.FromBitmap(Resourcen.mountains);
			texBackgroundMipmap.Filter = TextureFilterMode.Mipmap; //filter by calculating the pixels color as a weighted average of the neighboring texel's colors
			texBackgroundMipmap.WrapFunction = TextureWrapFunction.Repeat;
			//background clear color
			GL.ClearColor(Color.Black);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw with different filter modes - defines how texture colors are mapped to pixel colors
			DrawTexturedRect(new Box2D(-1, -1, 1, 2), texBackgroundLinear, texCoord);
			DrawTexturedRect(new Box2D(0, -1, 1, 2), texBackgroundMipmap, texCoord);
		}

		private void Update(float updatePeriod)
		{
			if (texCoord.SizeX > 200f || texCoord.SizeX < 1f) scaleFactor = -scaleFactor;
			float factor = scaleFactor * updatePeriod;
			texCoord.SizeX *= 1 + factor;
			texCoord.SizeY *= 1 + factor;
			//texCoord.CenterX += factor * 0.1f;
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			var visual = new MyVisual();
			app.Render += visual.Render;
			app.Update += visual.Update;
			app.Run();
		}

		private static void DrawTexturedRect(Box2D rect, ITexture tex, Box2D texCoords)
		{
			tex.Activate();
			rect.DrawTexturedRect(texCoords);
			tex.Deactivate();
		}
	}
}