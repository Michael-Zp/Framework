using OpenTK.Graphics.OpenGL4;
using Zenseless.Base;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	public class TextureToFrameBuffer : Disposable
	{
		public delegate void SetUniforms(IShader currentShader);

		public TextureToFrameBuffer(string fragmentShader = DefaultShader.FragmentShaderCopy, string vertexShader = DefaultShader.VertexShaderScreenQuad)
		{
			shader = ShaderLoader.FromStrings(vertexShader, fragmentShader);
		}

		public void Draw(ITexture texture, SetUniforms setUniformsHandler = null)
		{
			shader.Activate();
			texture.Activate();
			setUniformsHandler?.Invoke(shader);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			texture.Deactivate();
			shader.Deactivate();
		}

		private IShader shader;

		protected override void DisposeResources()
		{
			shader.Dispose();
		}
	}
}
