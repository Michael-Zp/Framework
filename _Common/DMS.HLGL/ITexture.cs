using System;

namespace DMS.HLGL
{
	public enum TextureFilterMode { Nearest, Linear, Mipmap };
	public enum TextureWrapFunction { Repeat, MirroredRepeat, ClampToEdge, ClampToBorder };

	public interface ITexture : IDisposable
	{
		TextureFilterMode Filter { get; set; }
		int Height { get; }
		uint ID { get; }
		int Width { get; }
		TextureWrapFunction WrapFunction { get; set; }

		void Activate();
		void Deactivate();
		void LoadPixels(IntPtr pixels, int width, int height, byte components = 4, bool floatingPoint = false);
	}
}