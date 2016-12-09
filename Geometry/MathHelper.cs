using System;
using System.Numerics;

namespace Geometry
{
	public static class MathHelper
	{
		public static float TWO_PI = (float)Math.PI * 2.0f;

		public static float Clamp(float x, float min, float max)
		{
			return Math.Min(max, Math.Max(min, x));
		}

		public static float DegreesToRadians(float angle)
		{
			return (angle * TWO_PI) / 360.0f;
		}

		/// <summary>
		/// Copy matrix elements into array in column major style
		/// </summary>
		/// <param name="input">matrix to convert</param>
		/// <returns>array of matrix elements</returns>
		public static float[] ToArray(this Matrix4x4 input)
		{
			int i = 0;
			var a = new float[16];
			
			a[i++] = input.M11;
			a[i++] = input.M21;
			a[i++] = input.M31;
			a[i++] = input.M41;

			a[i++] = input.M12;
			a[i++] = input.M22;
			a[i++] = input.M32;
			a[i++] = input.M42;

			a[i++] = input.M13;
			a[i++] = input.M23;
			a[i++] = input.M33;
			a[i++] = input.M43;

			a[i++] = input.M14;
			a[i++] = input.M24;
			a[i++] = input.M34;
			a[i++] = input.M44;

			return a;
		}
	}
}
