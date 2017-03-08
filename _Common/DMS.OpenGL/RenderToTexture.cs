using OpenTK.Graphics.OpenGL;

namespace DMS.OpenGL
{
	public class RenderToTexture
	{
		public RenderToTexture(Texture texture)
		{
			Texture = texture;
		}

		public void Activate()
		{
			GL.PushAttrib(AttribMask.ViewportBit);
			fbo.Activate(Texture); //start drawing into texture
			GL.Viewport(0, 0, Texture.Width, Texture.Height);
		}

		public void Deactivate()
		{
			fbo.Deactivate();
			GL.PopAttrib();
		}

		private FBO fbo = new FBO();
		public Texture Texture;
	}
}
