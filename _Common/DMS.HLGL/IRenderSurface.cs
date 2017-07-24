namespace DMS.HLGL
{
	public interface IRenderSurface
	{
		ITexture Texture { get; }
		void Clear();
		void Draw(IDrawConfiguration config);
	}
}