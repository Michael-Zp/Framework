using System;

namespace ShaderForm
{
	public interface IShaderFile : IDisposable
	{
		event EventHandler<string> Changed;

		void Load(string shaderFileName);
	}
}