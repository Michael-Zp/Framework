using DMS.System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace DMS.OpenGL
{
	/// <summary>
	/// Writes rendered data into bitmaps. Usefull for creation of videos
	/// </summary>
	public class FrameListCreator : Disposable
	{
		public FrameListCreator(int width, int height, PixelFormat format = PixelFormat.Format24bppRgb, bool drawToFrameBuffer = true)
		{
			this.format = format;
			render2tex = new RenderToTexture(Texture.Create(width, height,
				TextureLoader.SelectInternalPixelFormat(format),
				TextureLoader.SelectInputPixelFormat(format)));
			if(drawToFrameBuffer) tex2fb = new TextureToFrameBuffer();
		}

		public void Activate()
		{
			render2tex.Activate();
		}

		public void Deactivate()
		{
			render2tex.Deactivate();
			var bmp = TextureLoader.SaveToBitmap(render2tex.Texture, format);
			frames.Add(bmp);
			if (!ReferenceEquals(null, tex2fb))
			{
				//if blending is used we have to clear the framebuffer
				//if depth testing is used
				OpenTK.Graphics.OpenGL.GL.PushAttrib(OpenTK.Graphics.OpenGL.AttribMask.ColorBufferBit);
				OpenTK.Graphics.OpenGL.GL.PushAttrib(OpenTK.Graphics.OpenGL.AttribMask.DepthBufferBit);
				OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
				OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);
				//OpenTK.Graphics.OpenGL.GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit);
				tex2fb.Draw(render2tex.Texture);
				OpenTK.Graphics.OpenGL.GL.PopAttrib();
				OpenTK.Graphics.OpenGL.GL.PopAttrib();
			}
		}

		protected override void DisposeResources()
		{
			render2tex.Dispose();
			tex2fb.Dispose();
		}

		public IEnumerable<Bitmap> Frames { get { return frames; } }

		private RenderToTexture render2tex;
		private TextureToFrameBuffer tex2fb;
		private List<Bitmap> frames = new List<Bitmap>();
		private PixelFormat format;
	}
}
