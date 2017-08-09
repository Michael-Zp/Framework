using DMS.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;

namespace DMS.OpenGL
{
	public class Texture2dGL : Texture, ITexture2D
	{
		public int Width { get; private set; } = 0;
		public int Height { get; private set; } = 0;

		public Texture2dGL(): base(TextureTarget.Texture2D) { }

		public static Texture2dGL Create(int width, int height, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			return Create(width, height, internalFormat, inputPixelFormat, type);
		}

		public static Texture2dGL Create(int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat = PixelFormat.Rgba, PixelType type = PixelType.UnsignedByte)
		{
			var texture = new Texture2dGL();
			//create empty texture of given size
			texture.LoadPixels(IntPtr.Zero, width, height, internalFormat, inputPixelFormat, type);
			//set default parameters for filtering and clamping
			texture.Filter = TextureFilterMode.Linear;
			texture.WrapFunction = TextureWrapFunction.Repeat;
			return texture;
		}

		public void LoadPixels(IntPtr pixels, int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat, PixelType type)
		{
			Activate();
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			Deactivate();
		}

		public void LoadPixels(IntPtr pixels, int width, int height, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			Activate();
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			Deactivate();
		}
	}
}
