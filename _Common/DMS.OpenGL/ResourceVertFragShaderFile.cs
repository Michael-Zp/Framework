using DMS.HLGL;
using DMS.OpenGL;
using System;

namespace DMS.Application
{
	public class ResourceVertFragShaderFile : IResource<IShader>
	{
		public ResourceVertFragShaderFile(string sVertexShdFile_, string sFragmentShdFile_)
		{
			shader = ShaderLoader.FromFiles(sVertexShdFile_, sFragmentShdFile_);
		}

		public bool IsValueCreated { get { return !ReferenceEquals(null, shader); } }

		public IShader Value { get { return shader; } }

		public event EventHandler<IShader> Change { add { } remove { } }

		private IShader shader;
	}
}
