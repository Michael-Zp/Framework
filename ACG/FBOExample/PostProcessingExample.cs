using DMSOpenGL;
using OpenTK.Graphics.OpenGL;
using System.Text;

namespace Example
{
	public class PostProcessingExample
	{
		private FBO fbo;
		private Texture textureForRendering;
		private Shader shaderPostProcess;
		private Shader shaderSource;

		public PostProcessingExample(int width, int height)
		{
			fbo = new FBO();
			textureForRendering = Texture.Create(width, height);
			shaderPostProcess = PixelShader.Create(Encoding.UTF8.GetString(Resources.Swirl));
			shaderSource = PixelShader.Create(Encoding.UTF8.GetString(Resources.PatternCircle));
		}

		public void Draw(bool doPostProcessing, int width, int height, float time)
		{
			if (doPostProcessing)
			{
				fbo.BeginUse(textureForRendering); //start drawing into texture
				GL.Viewport(0, 0, textureForRendering.Width, textureForRendering.Height);
			}

			//draw staff
			shaderSource.Activate();
			GL.Uniform2(shaderSource.GetUniformLocation("iResolution"), (float)width, (float)height);
			GL.Uniform1(shaderSource.GetUniformLocation("iGlobalTime"), time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderSource.Deactivate();

			if (doPostProcessing)
			{
				fbo.EndUse(); //stop drawing into texture
				GL.Viewport(0, 0, width, height);
				textureForRendering.Activate();
				shaderPostProcess.Activate();
				GL.Uniform2(shaderPostProcess.GetUniformLocation("iResolution"), (float)width, (float)height);
				GL.Uniform1(shaderPostProcess.GetUniformLocation("iGlobalTime"), time);
				GL.DrawArrays(PrimitiveType.Quads, 0, 4);
				shaderPostProcess.Deactivate();
				textureForRendering.Deactivate();
			}
		}
	}
}
