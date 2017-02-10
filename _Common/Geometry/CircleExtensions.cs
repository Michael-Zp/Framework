using System;
using System.Numerics;

namespace Geometry
{
	public static class CircleExtensions
	{
		public static Circle CreateFromBox(Box2D box)
		{
			var circle = new Circle(box.CenterX, box.CenterY, 0.5f * Math.Min(box.SizeX, box.SizeY));
			return circle;
		}

		public static Circle CreateFromMinMax(float minX, float minY, float maxX, float maxY)
		{
			var box = Box2dExtensions.CreateFromMinMax(minX, minY, maxX, maxY);
			return CreateFromBox(box);
		}

		public static void UndoOverlap(this Circle a, Circle b)
		{
			Vector2 cB = new Vector2(b.CenterX, b.CenterY);
			Vector2 diff = new Vector2(a.CenterX, a.CenterY);
			diff -= cB;
			diff /= diff.Length();
			diff *= a.Radius + b.Radius;
			var newA = cB + diff;
			a.CenterX = newA.X;
			a.CenterY = newA.Y;
		}
	}
}
