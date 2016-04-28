namespace Framework
{
	public static class AABRextensions
	{
		public static bool PushXRangeInside(this AABR frameA, AABR frameB)
		{
			if (frameA.SizeX > frameB.SizeX) return false;
			if (frameA.X < frameB.X)
			{
				frameA.X = frameB.X;
			}
			if (frameA.MaxX > frameB.MaxX)
			{
				frameA.MaxX = frameB.MaxX;
			}
			return true;
		}

		public static bool PushYRangeInside(this AABR frameA, AABR frameB)
		{
			if (frameA.SizeY > frameB.SizeY) return false;
			if (frameA.Y < frameB.Y)
			{
				frameA.Y = frameB.Y;
			}
			if (frameA.MaxY > frameB.MaxY)
			{
				frameA.MaxY = frameB.MaxY;
			}
			return true;
		}

		/// <summary>
		/// Calculates the AABR in the overlap
		/// Returns null if no intersection
		/// </summary>
		/// <param name="frameB"></param>
		/// <returns>AABR in the overlap</returns>
		public static AABR Overlap(this AABR frameA, AABR frameB)
		{
			AABR overlap = null;

			if (frameA.Intersects(frameB))
			{
				overlap = new AABR(0.0f, 0.0f, 0.0f, 0.0f);

				overlap.X = (frameA.X < frameB.X) ? frameB.X : frameA.X;
				overlap.Y = (frameA.Y < frameB.Y) ? frameB.Y : frameA.Y;

				overlap.SizeX = (frameA.MaxX < frameB.MaxX) ? frameA.MaxX - overlap.X : frameB.MaxX - overlap.X;
				overlap.SizeY = (frameA.MaxY < frameB.MaxY) ? frameA.MaxY - overlap.Y : frameB.MaxY - overlap.Y;
			}

			return overlap;
		}
	}
}
