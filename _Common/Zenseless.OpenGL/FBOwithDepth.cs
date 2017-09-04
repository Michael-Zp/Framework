using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	public class FBOwithDepth : FBO
	{
		public FBOwithDepth(ITexture2D texture) : base(texture)
		{
			Activate();
			depth = new RenderBuffer(RenderbufferStorage.DepthComponent32, texture.Width, texture.Height);
			depth.Attach(FramebufferAttachment.DepthAttachment);
			Deactivate();
		}

		protected override void DisposeResources()
		{
			base.DisposeResources();
			depth.Dispose();
		}

		private RenderBuffer depth;
	}
}