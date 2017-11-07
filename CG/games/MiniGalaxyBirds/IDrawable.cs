using Zenseless.Geometry;

namespace MiniGalaxyBirds
{
	public interface IDrawable
	{
		IImmutableBox2D Rect { get; }
		void Draw();
	}
}
