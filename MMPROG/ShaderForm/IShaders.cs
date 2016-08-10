using System;
using System.Collections.Generic;

namespace ShaderForm
{
	public interface IShaders : IEnumerable<string>
	{
		event EventHandler<string> OnChange;

		void AddUpdateShader(string shaderFileName);
		void Clear();
		void RemoveShader(string shaderFileName);
	}
}