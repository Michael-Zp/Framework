using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	public class PostProcessing
	{
		public PostProcessing(int width, int height)
		{
			rtt = new RenderToTexture(Texture.Create(width, height));
			SetShader(TextureToFrameBuffer.FragmentShaderCopy);
		}

		public void Start()
		{
			rtt.Activate();
		}

		public void EndAndApply(int width, int height, float time = 0.0f)
		{
			rtt.Deactivate();
			t2fb.Draw(rtt.Texture, (shader) =>
				{
					GL.Uniform2(shader.GetUniformLocation("iResolution"), (float)width, (float)height);
					GL.Uniform1(shader.GetUniformLocation("iGlobalTime"), time);
					//GL.Uniform1(shader.GetUniformLocation("amplitude"), 0.01f);
				}
			);
		}
		public void EndAndApply(float time = 0.0f)
		{
			EndAndApply(rtt.Texture.Width, rtt.Texture.Height, time);
		}

		public void SetShader(string fragmentShaderText)
		{
			t2fb = new TextureToFrameBuffer(fragmentShaderText);
		}

		private RenderToTexture rtt;
		private TextureToFrameBuffer t2fb;
	}
}
