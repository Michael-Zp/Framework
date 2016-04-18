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

		public float CenterX { get { return X + 0.5f * SizeX; }  set { X = value - 0.5f * SizeX; } }

		public float CenterY { get { return Y + 0.5f * SizeY; } set { Y = value - 0.5f * SizeY; } }

		public bool Intersects(AABR frame)
		{
			if (null == frame) return false;
			bool noXintersect = (MaxX < frame.X) || (X > frame.MaxX);
			bool noYintersect = (MaxY < frame.Y) || (Y > frame.MaxY);
			return !(noXintersect || noYintersect);
		}

		/// <summary>
		/// Calculates the AABR in the overlap
		/// Returns null if no intersection
		/// </summary>
		/// <param name="frame"></param>
		/// <returns>AABR in the overlap</returns>
		public AABR Overlap(AABR frame)
		{
			AABR overlap = null;

			if (Intersects(frame))
			{
				overlap = new AABR(0.0f, 0.0f, 0.0f, 0.0f);

				overlap.X = (X < frame.X) ? frame.X : X;
				overlap.Y = (Y < frame.Y) ? frame.Y : Y;

				overlap.SizeX = (MaxX < frame.MaxX) ? MaxX - overlap.X : frame.MaxX - overlap.X;
				overlap.SizeY = (MaxY < frame.MaxY) ? MaxY - overlap.Y : frame.MaxY - overlap.Y;
			}

			return overlap;
		}

		public bool PushXRangeInside(AABR frame)
		{
			if (SizeX > frame.SizeX) return false;
			if (X < frame.X)
			{
				X = frame.X;
			}
			if (MaxX > frame.MaxX)
			{
				MaxX = frame.MaxX;
			}
			return true;
		}

		public bool PushYRangeInside(AABR frame)
		{
			if (SizeY > frame.SizeY) return false;
			if (Y < frame.Y)
			{
				Y = frame.Y;
			}
			if (MaxY > frame.MaxY)
			{
				MaxY = frame.MaxY;
			}
			return true;
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
