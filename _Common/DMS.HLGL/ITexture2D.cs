using System;

namespace DMS.HLGL
{
	public interface ITexture2D : ITexture
	{
		int Height { get; }
		int Width { get; }

		void LoadPixels(IntPtr pixels, int width, int height, byte components = 4, bool floatingPoint = false);
	}
}