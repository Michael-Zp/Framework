using Zenseless.Base;
using Zenseless.Geometry;

namespace MiniGalaxyBirds
{
	public class ComponentClipper : IComponent, ITimedUpdate
	{
		public ComponentClipper(IImmutableBox2D clipFrame, IImmutableBox2D frame, Clip clip)
		{
			this.ClipFrame = clipFrame;
			this.Frame = frame;
			this.OnClip = clip;
		}

		public void Update(float absoluteTime)
		{
			if (!ReferenceEquals(null, OnClip) && !this.ClipFrame.Intersects(this.Frame))
			{
				OnClip();
			}
		}

		public IImmutableBox2D ClipFrame { get; private set; }
		public IImmutableBox2D Frame { get; private set; }

		public delegate void Clip();
		public event Clip OnClip;
	}
}
