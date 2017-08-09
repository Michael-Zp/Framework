using OpenTK.Graphics.OpenGL4;
using System;

namespace DMS.OpenGL
{
	public class TextureArrayGL : Texture
	{
		public int Width { get; private set; } = 0;
		public int Height { get; private set; } = 0;
		public int Elements { get; private set; } = 1;

		public TextureArrayGL(): base(TextureTarget.Texture2DArray) { }

		public void LoadPixels(IntPtr pixels, int element, byte components = 4, bool floatingPoint = false)
		{
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			Activate();
			GL.TexSubImage3D(Target, 0, 0, 0, 0, Width, Height, element, inputPixelFormat, type, pixels);
			Deactivate();
		}

		public void SetFormat(int width, int height, int elements, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			Activate();
			GL.TexStorage3D(TextureTarget3d.Texture2DArray, 0, SizedInternalFormat.R16, width, height, elements);
			this.Width = width;
			this.Height = height;
			this.Elements = elements;
			Deactivate();
		}
	}
}
