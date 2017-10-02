using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;
using System.Drawing;

namespace Zenseless.Application
{
	public class ResourceTextureBitmap : IResource<ITexture>
	{
		public ResourceTextureBitmap(Bitmap bitmap)
		{
			texture = TextureLoader.FromBitmap(bitmap);
		}

		public bool IsValueCreated { get { return true;  } }

		public ITexture Value { get { return texture; } }

		public event EventHandler<ITexture> Change {  add { } remove { } }

		private ITexture texture;
	}
}
