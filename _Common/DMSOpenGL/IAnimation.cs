using Geometry;

namespace DMSOpenGL
{
	public interface IAnimation
	{
		float AnimationLength { get; set; }

		void Draw(Box2D rectangle, float totalSeconds);
	}
}