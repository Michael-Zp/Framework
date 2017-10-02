﻿using Zenseless.Geometry;

namespace MiniGalaxyBirds
{
	public interface IDrawable
	{
		Box2D Rect { get; }
		void Draw();
	}
}
