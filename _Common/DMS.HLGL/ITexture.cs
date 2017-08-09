using System;

namespace DMS.HLGL
{
	public enum TextureFilterMode { Nearest, Linear, Mipmap };
	public enum TextureWrapFunction { Repeat, MirroredRepeat, ClampToEdge, ClampToBorder };

	public interface ITexture : IDisposable
	{
		TextureFilterMode Filter { get; set; }
		uint ID { get; }
		TextureWrapFunction WrapFunction { get; set; }

		void Activate();
		void Deactivate();
	}
}