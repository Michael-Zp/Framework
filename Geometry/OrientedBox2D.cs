using System;
using OpenTK;

namespace Geometry
{
	public class OrientedBox2D
	{
		public OrientedBox2D(float centerX, float centerY, float sizeX, float sizeY, float angle)
		{
			center.X = centerX;
			center.Y = centerY;
			Angle = angle;
			Radii = new Vector2(sizeX / 2.0f, sizeY / 2.0f);
			CalcHelpers();
		}

		public Vector2 Center { get; set; }
		public float CenterX { get { return center.X; } set { center.X = value; } }
		public float CenterY { get { return center.Y; } set { center.Y = value; } }

		public float Angle { get; set; }

		public bool Intersects(OrientedBox2D other)
		{
			return false;
		}

		public Vector2 Radii { get; set; }

		private Vector2 center;

		private Matrix2 basis;
		private Vector2[] corner = new Vector2[4];

		private void CalcHelpers()
		{
			Matrix2.CreateRotation(Angle, out basis);
			var S = Matrix2.CreateScale(Radii);
			var axis = S * basis;

			corner[0] = center - axis.Column0 - axis.Column1;
			corner[1] = center + axis.Column0 - axis.Column1;
			corner[2] = center + axis.Column0 + axis.Column1;
			corner[3] = center - axis.Column0 + axis.Column1;
		}

		//private bool Overlaps1Way(OrientedBox2D other)
		//{
		//	for (int axis = 0; axis < 2; ++axis)
		//	{
		//		float t = Vector2.Dot(other.corner[0], axis
		//	}
		//}
	}
}
