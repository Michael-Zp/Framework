using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	/// <summary>
	/// Represents an axis aligned bounding box - naming it rectangle would have been too simple ;)
	/// </summary>
	public class AABR
	{
		/// <summary>
		/// creates an AABR
		/// </summary>
		/// <param name="x">left side x coordinate</param>
		/// <param name="y">bottom side y coordinate</param>
		/// <param name="sizeX">width</param>
		/// <param name="sizeY">height</param>
		public AABR(float x, float y, float sizeX, float sizeY)
		{
			this.X = x;
			this.Y = y;
			this.SizeX = sizeX;
			this.SizeY = sizeY;
		}

		public AABR(AABR aabr)
		{
			this.X = aabr.X;
			this.Y = aabr.Y;
			this.SizeX = aabr.SizeX;
			this.SizeY = aabr.SizeY;
		}

		public float SizeX { get; set; }

		public float SizeY { get; set; }

		public float X { get; set; }

		public float Y { get; set; }

		public float CenterX { get { return X + 0.5f * SizeX; } set { X = value - 0.5f * SizeX; } }

		public float CenterY { get { return Y + 0.5f * SizeY; } set { Y = value - 0.5f * SizeY; } }

		public bool Intersects(AABR frame)
		{
			if (null == frame) return false;
			bool noXintersect = (MaxX < frame.X) || (X > frame.MaxX);
			bool noYintersect = (MaxY < frame.Y) || (Y > frame.MaxY);
			return !(noXintersect || noYintersect);
		}

		public bool Inside(AABR frame)
		{
			if (X < frame.X) return false;
			if (MaxX > frame.MaxX) return false;
			if (Y < frame.Y) return false;
			if (MaxY > frame.MaxY) return false;
			return true;
		}

		public float MaxX { get { return X + SizeX; } set { X = value - SizeX; } }

		public float MaxY { get { return Y + SizeY; } set { Y = value - SizeY; } }

		public override string ToString()
		{
			return '(' + X.ToString() + ';' + Y.ToString() + ';' + SizeX.ToString() + ';' + SizeY.ToString() + ')';
		}

	}
}
