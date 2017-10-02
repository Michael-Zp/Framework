﻿using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Example
{
	public class Rasterizer
	{
		public delegate void DrawHandler();

		public Rasterizer(int resolutionX, int resolutionY, DrawHandler drawHandler)
		{
			if (ReferenceEquals(null, drawHandler)) throw new ArgumentException("Draw handler must not equal null!");
			this.drawHandler = drawHandler;
			copyToFrameBuffer = new TextureToFrameBuffer();
			texRenderSurface = Texture2dGL.Create(resolutionX, resolutionY);
			texRenderSurface.Filter = TextureFilterMode.Nearest;
			renderToTexture = new FBO(texRenderSurface);
		}

		private ITexture2D texRenderSurface;
		private FBO renderToTexture;
		private TextureToFrameBuffer copyToFrameBuffer;
		private DrawHandler drawHandler;

		public void Render()
		{
			renderToTexture.Activate();
			drawHandler();
			renderToTexture.Deactivate();
			copyToFrameBuffer.Draw(texRenderSurface);
		}
	}
}
