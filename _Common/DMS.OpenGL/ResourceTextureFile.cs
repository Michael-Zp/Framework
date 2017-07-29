using DMS.HLGL;
using DMS.OpenGL;
using System;

namespace DMS.Application
{
	public class ResourceTextureFile : IResource<ITexture>
	{
		public ResourceTextureFile(string filePath)
		{
			texture = TextureLoader.FromFile(filePath);
		}

		public bool IsValueCreated { get { return true; } }

		public ITexture Value { get { return texture; } }

		public event EventHandler<ITexture> Change { add { } remove { } }

		private ITexture texture;
	}
}
