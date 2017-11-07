using Zenseless.Geometry;

namespace Zenseless.OpenGL
{
	public interface IAnimation
	{
		float AnimationLength { get; set; }

		void Draw(IImmutableBox2D rectangle, float totalSeconds);
	}
}