using System;

namespace ShaderForm
{
	//todo: add tracks, sum
	public class DemoModel : IDisposable
	{
		public delegate void SetUniformsHandler(ISetUniform visualContext);
		public event SetUniformsHandler OnSetCustomUniforms;
		public ShaderKeyframes ShaderKeyframes { get; private set; }
		public IShaders Shaders { get; private set; }
		public ITextures Textures { get; private set; }
		public IUniforms Uniforms { get { return uniforms; } }
		public DemoTimeSource TimeSource { get; private set; }

		public DemoModel(VisualContext visualContext, IShaders shaders, ITextures textures, bool isLooping)
		{
			uniforms = new Uniforms();
			ShaderKeyframes = new ShaderKeyframes();
			TimeSource = new DemoTimeSource(isLooping);

			this.visualContext = visualContext;
			Shaders = shaders;
			Textures = textures;
		}

		public void Clear()
		{
			TimeSource.Clear();
			Shaders.Clear();
			Textures.Clear();
			uniforms.Clear();
			ShaderKeyframes.Clear();
		}

		public void Dispose()
		{
			Clear();
			visualContext.Dispose();
		}

		public void Draw(int width, int height)
		{
			visualContext.Draw(width, height);
		}

		public void SaveBuffer(string fileName)
		{
			visualContext.Save(fileName);
		}

		public void UpdateBuffer(int mouseX, int mouseY, bool leftButton, int bufferWidth, int bufferHeight)
		{
			visualContext.UpdateSurfaceSize(bufferWidth, bufferHeight);

			var currentShader = ShaderKeyframes.GetCurrentShader(TimeSource.Position);
			visualContext.SetShader(currentShader);
			visualContext.SetUniform("iGlobalTime", TimeSource.Position);
			visualContext.SetUniform("iResolution", bufferWidth, bufferHeight);
			visualContext.SetUniform("iMouse", mouseX, mouseY, leftButton ? 1.0f : 0.0f);

			uniforms.Interpolate(TimeSource.Position, (name, value) => visualContext.SetUniform(name, value));

			//override with custom uniforms
			OnSetCustomUniforms?.Invoke(visualContext);

			visualContext.Update();
		}

		private readonly VisualContext visualContext;
		private Uniforms uniforms;
	}
}
