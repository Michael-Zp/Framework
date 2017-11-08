using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Transformation 2D class that is based on row-major matrices
	/// </summary>
	public class Transformation2D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Transformation2D"/> class.
		/// </summary>
		public Transformation2D()
		{
			Reset();
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="Transformation2D"/> to <see cref="Matrix3x2"/>.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator Matrix3x2(Transformation2D t)
		{
			return t.matrix;
		}

		/// <summary>
		/// create a rotation matrix that rotates around a given rotation center (pivot point)
		/// </summary>
		/// <param name="pivotX">rotation center x</param>
		/// <param name="pivotY">rotation center y</param>
		/// <param name="degrees">rotation in degrees</param>
		/// <returns></returns>
		public static Transformation2D CreateRotationAround(float pivotX, float pivotY, float degrees)
		{
			var t = new Transformation2D();
			t.TranslateGlobal(-pivotX, -pivotY);
			t.RotateGlobal(degrees);
			t.TranslateGlobal(pivotX, pivotY);
			return t;
		}

		/// <summary>
		/// Creates the scale around.
		/// </summary>
		/// <param name="pivotX">The pivot x.</param>
		/// <param name="pivotY">The pivot y.</param>
		/// <param name="scaleX">The scale x.</param>
		/// <param name="scaleY">The scale y.</param>
		/// <returns></returns>
		public static Transformation2D CreateScaleAround(float pivotX, float pivotY, float scaleX, float scaleY)
		{
			var t = new Transformation2D();
			t.TranslateGlobal(-pivotX, -pivotY);
			t.ScaleGlobal(scaleX, scaleY);
			t.TranslateGlobal(pivotX, pivotY);
			return t;
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		public void Reset()
		{
			matrix = Matrix3x2.Identity;
		}

		/// <summary>
		/// Rotate Transform
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateGlobal(float degrees)
		{
			TransformGlobal(Matrix3x2.CreateRotation(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Rotates the local.
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateLocal(float degrees)
		{
			TransformLocal(Matrix3x2.CreateRotation(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Scales the global.
		/// </summary>
		/// <param name="scales">The scales.</param>
		public void ScaleGlobal(Vector2 scales)
		{
			TransformGlobal(Matrix3x2.CreateScale(scales));
		}

		/// <summary>
		/// Scales the global.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void ScaleGlobal(float x, float y)
		{
			TransformGlobal(Matrix3x2.CreateScale(x, y));
		}

		/// <summary>
		/// Scales the local.
		/// </summary>
		/// <param name="scales">The scales.</param>
		public void ScaleLocal(Vector2 scales)
		{
			TransformLocal(Matrix3x2.CreateScale(scales));
		}

		/// <summary>
		/// Scales the local.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void ScaleLocal(float x, float y)
		{
			TransformLocal(Matrix3x2.CreateScale(x, y));
		}

		/// <summary>
		/// Translates the global.
		/// </summary>
		/// <param name="translation">The translation.</param>
		public void TranslateGlobal(Vector2 translation)
		{
			TransformGlobal(Matrix3x2.CreateTranslation(translation));
		}

		/// <summary>
		/// Translates the global.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void TranslateGlobal(float x, float y)
		{
			TransformGlobal(Matrix3x2.CreateTranslation(x, y));
		}

		/// <summary>
		/// Translates the local.
		/// </summary>
		/// <param name="translation">The translation.</param>
		public void TranslateLocal(Vector2 translation)
		{
			TransformLocal(Matrix3x2.CreateTranslation(translation));
		}

		/// <summary>
		/// Translates the local.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void TranslateLocal(float x, float y)
		{
			TransformLocal(Matrix3x2.CreateTranslation(x, y));
		}

		/// <summary>
		/// Transforms the specified position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns></returns>
		public Vector2 Transform(Vector2 position)
		{
			return Vector2.Transform(position, matrix);
		}

		/// <summary>
		/// Transforms the global.
		/// </summary>
		/// <param name="transform">The transform.</param>
		public void TransformGlobal(Matrix3x2 transform)
		{
			matrix *= transform;
		}

		/// <summary>
		/// Transforms the local.
		/// </summary>
		/// <param name="transform">The transform.</param>
		public void TransformLocal(Matrix3x2 transform)
		{
			matrix = transform * matrix;
		}

		/// <summary>
		/// The matrix
		/// </summary>
		private Matrix3x2 matrix;
	}
}
