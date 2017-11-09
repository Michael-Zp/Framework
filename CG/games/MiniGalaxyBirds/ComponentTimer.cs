﻿using Zenseless.Base;

namespace MiniGalaxyBirds
{
	public class ComponentPeriodicUpdate : PeriodicUpdate, IComponent
	{
		public ComponentPeriodicUpdate(float startTime, float interval) : base(interval)
		{
			Start(startTime);
		}
	}
}
