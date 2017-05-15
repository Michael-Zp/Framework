using System;
using OpenTK.Graphics.OpenGL;
using DMS.Base;

namespace DMS.OpenGL
{
	public class RenderToTexture : Disposable
	{
		public RenderToTexture(Texture texture, bool depthBuffer = false)
		{
			this.texture = texture;
			if (depthBuffer)
			{
				fbo.Activate(texture);
				depth = GL.GenRenderbuffer();
				GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depth);
				GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, texture.Width, texture.Height);
				GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.Depth, RenderbufferTarget.Renderbuffer, depth);
				//GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
				fbo.Deactivate();
			}
		}

		public Texture Texture { get { return texture; } }

		public void Activate()
		{
			fbo.Activate(Texture); //start drawing into texture
			if(-1 != depth)
			{ 
			}
		}

		public void Deactivate()
		{
			fbo.Deactivate();
		}

		protected override void DisposeResources()
		{
			fbo.Dispose();
			Texture.Dispose();
		}

		private FBO fbo = new FBO();
		private int depth = -1;
		private Texture texture;
	}
}
