using System.Numerics;

namespace Geometry
{
	/// <summary>
	/// Some ROW vector transformation operations, to get the column vector version transpose
	/// </summary>
	public static class Transform2D
	{
		/// <summary>
		/// create a rotation matrix that rotates around a given rotation center (pivot point)
		/// </summary>
		/// <param name="angle">in radiants</param>
		/// <returns></returns>
		public static Matrix3x2 CreateRotationAroundOrigin(float angle)
		{
			return Matrix3x2.CreateRotation(angle);
		}

	/// <summary>
	/// create a rotation matrix that rotates around a given rotation center (pivot point)
	/// </summary>
	/// <param name="pivotX">rotation center x</param>
	/// <param name="pivotY">rotation center y</param>
	/// <param name="angle">radiant</param>
	/// <returns></returns>
	public static Matrix3x2 CreateRotationAround(float pivotX, float pivotY, float angle)
		{
			var Cinv = Matrix3x2.CreateTranslation(-pivotX, -pivotY);
			var R = CreateRotationAroundOrigin(angle);
			var C = Matrix3x2.CreateTranslation(pivotX, pivotY);
			return Cinv * R * C;
		}

		public static Matrix3x2 CreateScaleAroundOrigin(float scaleX, float scaleY)
		{
			return Matrix3x2.CreateScale(scaleX, scaleY);
		}

		public static Matrix3x2 CreateScaleAround(float pivotX, float pivotY, float scaleX, float scaleY)
		{
			var Cinv = Matrix3x2.CreateTranslation(-pivotX, -pivotY);
			var S = CreateScaleAroundOrigin(scaleX, scaleY);
			var C = Matrix3x2.CreateTranslation(pivotX, pivotY);
			return Cinv * S * C;
		}
	}
}
