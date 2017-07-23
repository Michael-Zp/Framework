namespace DMS.HLGL
{
	public interface IImage
	{
		ITexture Texture { get; }
		void Clear();
		void Draw(IDrawConfiguration config);
	}
}