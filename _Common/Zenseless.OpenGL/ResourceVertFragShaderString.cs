using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Zenseless.Application
{
	public class ResourceVertFragShaderString : IResource<IShader>
	{
		public ResourceVertFragShaderString(string sVertex, string sFragment)
		{
			shader = ShaderLoader.FromStrings(sVertex, sFragment);
		}

		public bool IsValueCreated { get { return true; } }

		public IShader Value { get { return shader; } }

		public event EventHandler<IShader> Change { add { } remove { } }

		private IShader shader;
	}
}