using Framework;

namespace MiniGalaxyBirds
{
	public class ComponentTimer : Timer, IComponent
	{
		public ComponentTimer(float startTime, float interval) : base(interval)
		{
			Update(startTime);
			Enabled = true;
		}
	}
}
