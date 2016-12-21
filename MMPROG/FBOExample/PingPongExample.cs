using Framework;
using OpenTK.Graphics.OpenGL;
using System.Text;

namespace Example
{
	public class PingPongExample
	{
		private FBO fbo;
		private Texture textureBufferA, textureBufferB, active;
		private Shader shaderCopy;
		private Shader shaderGameOfLife;

		public PingPongExample(int width, int height)
		{
			fbo = new FBO();
			textureBufferA = Texture.Create(width, height);
			textureBufferB = Texture.Create(width, height);
			active = textureBufferA;
			shaderCopy = PixelShader.Create(PixelShader.Copy);
			shaderGameOfLife = PixelShader.Create(Encoding.UTF8.GetString(Resources.GameOfLife));
		}

		public void Draw(int width, int height, float mouseX, float mouseY)
		{
			var last = (active == textureBufferA) ? textureBufferB : textureBufferA;

			fbo.BeginUse(active); //start drawing into texture
			GL.Viewport(0, 0, active.Width, active.Height);
			shaderGameOfLife.Begin();
			last.BeginUse();
			GL.Uniform2(shaderGameOfLife.GetUniformLocation("iResolution"), (float)width, (float)height);
			GL.Uniform2(shaderGameOfLife.GetUniformLocation("iMouse"), mouseX, mouseY);
			GL.Uniform1(shaderGameOfLife.GetUniformLocation("iSeedRadius"), 0.05f);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4); //draw staff
			last.EndUse();
			shaderGameOfLife.End();
			fbo.EndUse(); //stop drawing into texture


			GL.Viewport(0, 0, width, height);
			active.BeginUse();
			shaderCopy.Begin();
			GL.Uniform2(shaderCopy.GetUniformLocation("iResolution"), (float)width, (float)height);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderCopy.End();
			active.EndUse();

			active = last;
		}
	}
}
