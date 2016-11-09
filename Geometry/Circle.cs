namespace Geometry
{
	/// <summary>
	/// Represents a circle
	/// </summary>
	class Circle
	{
		public Circle(float centerX, float centerY, float radius)
		{
			CenterX = centerX;
			CenterY = centerY;
			Radius = radius;
		}

		public float CenterX { get; set; }
		public float CenterY { get; set; }
		public float Radius { get; set; }

		public bool Intersects(Circle circle)
		{
			if (null == circle) return false;
			var rr = circle.Radius + Radius;
			rr *= rr;
			var xx = circle.CenterX - CenterX;
			xx *= xx;
			var yy = circle.CenterY - CenterY;
			yy *= yy;
			return rr > xx + yy;  
		}
	}
}
