namespace MiniGalaxyBirds
{
	public class ComponentAnimated : ComponentTimer, IAnimation
	{
		public ComponentAnimated(float startTime, float length) : base(startTime, length) { }

		public float Length	{ get { return this.Interval; }	}

		public float Time { get { return this.ElapsedTime; } }
	}
}
