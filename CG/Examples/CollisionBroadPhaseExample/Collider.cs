using Geometry;
using System;
using System.Drawing;
using System.Numerics;

namespace Example
{
	public class Collider : IBox2DCollider
	{
		public Collider(float x, float y, float sizeX, float sizeY)
		{
			Box = new Box2D(x, y, sizeX, sizeY);
			var rndData = new byte[3];
			rnd.NextBytes(rndData);
			Color = Color.FromArgb(rndData[0], rndData[1], rndData[2]);
			Velocity = Vector2.Zero;
		}

		public Box2D Box { get; set; }
		public Color Color { get; private set; }
		public Vector2 Velocity { get; set; }

		public float MinX { get	{ return Box.X;	} }

		public float MinY { get { return Box.Y; } }

		public float MaxX { get { return Box.MaxX; } }

		public float MaxY { get { return Box.MaxY; } }

		public static Vector2 RndVelocity()
		{
			var rndData = new byte[2];
			rnd.NextBytes(rndData);
			var velocity = new Vector2(rndData[0], rndData[1]);
			velocity -= new Vector2(128, 128);
			velocity *= 0.001f;
			return velocity;
		}

		private static Random rnd = new Random(12);
	}
}
