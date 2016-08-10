using System;

namespace ShaderForm
{
	public interface IShaderFile : IDisposable
	{
		event EventHandler<string> OnChange;

		void Load(string shaderFileName);
	}
}