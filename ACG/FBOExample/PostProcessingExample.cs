using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System.Text;

namespace Example
{
	public class PostProcessingExample
	{
		private RenderToTexture rtt;
		private Shader shaderPostProcess;
		private Shader shaderSource;

		public PostProcessingExample(int width, int height)
		{
			rtt = new RenderToTexture(Texture.Create(width, height));
			shaderPostProcess = PixelShader.Create(Encoding.UTF8.GetString(Resources.Swirl));
			shaderSource = PixelShader.Create(Encoding.UTF8.GetString(Resources.PatternCircle));
		}

		public void Draw(bool doPostProcessing, int width, int height, float time)
		{
			if (doPostProcessing)
			{
				rtt.Activate(); //start drawing into texture
			}

			//draw staff
			shaderSource.Activate();
			GL.Uniform2(shaderSource.GetUniformLocation("iResolution"), (float)width, (float)height);
			GL.Uniform1(shaderSource.GetUniformLocation("iGlobalTime"), time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderSource.Deactivate();

			if (doPostProcessing)
			{
				rtt.Deactivate(); //stop drawing into texture
				rtt.Texture.Activate();
				shaderPostProcess.Activate();
				GL.Uniform2(shaderPostProcess.GetUniformLocation("iResolution"), (float)width, (float)height);
				GL.Uniform1(shaderPostProcess.GetUniformLocation("iGlobalTime"), time);
				GL.DrawArrays(PrimitiveType.Quads, 0, 4);
				shaderPostProcess.Deactivate();
				rtt.Texture.Deactivate();
			}
		}
	}
}
