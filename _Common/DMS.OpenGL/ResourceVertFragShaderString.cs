using DMS.HLGL;
using DMS.OpenGL;
using System;

namespace DMS.Application
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