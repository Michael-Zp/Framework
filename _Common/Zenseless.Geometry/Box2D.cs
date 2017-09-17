using System;
using System.Diagnostics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Represents an 2D axis aligned bounding box
	/// </summary>
	[Serializable]
	public class Box2D : IEquatable<Box2D>
	{
		/// <summary>
		/// creates an AABR, an 2D axis aligned bounding box
		/// </summary>
		/// <param name="minX">minimal x coordinate</param>
		/// <param name="minY">minimal y coordinate</param>
		/// <param name="sizeX">width</param>
		/// <param name="sizeY">height</param>
		public Box2D(float minX, float minY, float sizeX, float sizeY)
		{
			Debug.Assert(sizeX >= 0);
			Debug.Assert(sizeY >= 0);
			this.MinX = minX;
			this.MinY = minY;
			this.SizeX = sizeX;
			this.SizeY = sizeY;
		}

		public Box2D(Box2D rectangle)
		{
			this.MinX = rectangle.MinX;
			this.MinY = rectangle.MinY;
			this.SizeX = rectangle.SizeX;
			this.SizeY = rectangle.SizeY;
		}

		public static readonly Box2D BOX01 = new Box2D(0, 0, 1, 1);
		public static readonly Box2D EMPTY = new Box2D(0, 0, 0, 0);

		public float SizeX { get; set; }

		public float SizeY { get; set; }

		public float MinX { get; set; }

		public float MinY { get; set; }

		public float MaxX { get { return MinX + SizeX; } set { SizeX = value - MinX; } }

		public float MaxY { get { return MinY + SizeY; } set { SizeY = value - MinY; } }

		public float CenterX { get { return MinX + 0.5f * SizeX; } set { MinX = value - 0.5f * SizeX; } }

		public float CenterY { get { return MinY + 0.5f * SizeY; } set { MinY = value - 0.5f * SizeY; } }

		public static bool operator==(Box2D a, Box2D b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Box2D a, Box2D b)
		{
			return !a.Equals(b);
		}

		public bool Contains(float x, float y)
		{
			if (x < MinX || MaxX < x) return false;
			if (y < MinY || MaxY < y) return false;
			return true;
		}

		public bool Contains(Box2D rectangle)
		{
			if (MinX > rectangle.MinX) return false;
			if (MaxX < rectangle.MaxX) return false;
			if (MinY > rectangle.MinY) return false;
			if (MaxY < rectangle.MaxY) return false;
			return true;
		}

		public bool Equals(Box2D other)
		{
			if (ReferenceEquals(null, other)) return false;
			return MinX == other.MinX && MinY == other.MinY && SizeX == other.SizeX && SizeY == other.SizeY;
		}

		public override bool Equals(object other)
		{
			return Equals(other as Box2D);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool Intersects(Box2D rectangle)
		{
			bool noXintersect = (MaxX <= rectangle.MinX) || (MinX >= rectangle.MaxX);
			bool noYintersect = (MaxY <= rectangle.MinY) || (MinY >= rectangle.MaxY);
			return !(noXintersect || noYintersect);
		}

		public override string ToString()
		{
			return '(' + MinX.ToString() + ';' + MinY.ToString() + ';' + SizeX.ToString() + ';' + SizeY.ToString() + ')';
		}
	}
}
