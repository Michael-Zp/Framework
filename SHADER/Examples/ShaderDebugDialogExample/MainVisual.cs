using OpenTK.Graphics.OpenGL4;
using Zenseless.OpenGL;
using Zenseless.HLGL;

namespace Example
{
	public class MainVisual
	{
		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
		}

		public void Render()
		{
			if (ReferenceEquals(null, shader)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shader.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shader.Deactivate();
		}

		private IShader shader;
	}
}