namespace Framework
{
	public class AABR
	{
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

		public float CenterX { get { return X + 0.5f * SizeX; } }

		public float CenterY { get { return Y + 0.5f * SizeY; } }

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

        	public float MaxX { get { return X + SizeX; } }

		public float MaxY { get { return Y + SizeY; } }

		public override string ToString()
		{
			return '(' + X.ToString() + ';' + Y.ToString() + ';' + SizeX.ToString() + ';' + SizeY .ToString() + ')';
		}

	}
}
