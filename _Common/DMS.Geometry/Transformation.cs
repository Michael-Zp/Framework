using System.Numerics;

namespace DMS.Geometry
{
	/// <summary>
	/// todo: Row-major transformation class
	/// </summary>
	public class Transformation
	{
		public Transformation()
		{
			Reset();
		}

		public Matrix4x4 Matrix { get; private set; }

		public void Reset()
		{
			Matrix = Matrix4x4.Identity;
		}

		/// <summary>
		/// Rotate Transform
		/// </summary>
		/// <param name="degrees"></param>
		public void RotateXGlobal(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(degrees)));
		}

		public void RotateYGlobal(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(degrees)));
		}

		public void RotateZGlobal(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationZ(MathHelper.DegreesToRadians(degrees)));
		}

		public void RotateXLocal(float degrees)
		{
			TransformLocal(Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(degrees)));
		}

		public void RotateYLocal(float degrees)
		{
			TransformLocal(Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(degrees)));
		}

		public void RotateZLocal(float degrees)
		{
			TransformLocal(Matrix4x4.CreateRotationZ(MathHelper.DegreesToRadians(degrees)));
		}

		public void ScaleGlobal(Vector3 scales)
		{
			TransformGlobal(Matrix4x4.CreateScale(scales));
		}

		public void ScaleGlobal(float x, float y, float z)
		{
			TransformGlobal(Matrix4x4.CreateScale(x, y, z));
		}

		public void ScaleLocal(Vector3 scales)
		{
			TransformLocal(Matrix4x4.CreateScale(scales));
		}

		public void ScaleLocal(float x, float y, float z)
		{
			TransformLocal(Matrix4x4.CreateScale(x, y, z));
		}

		public void TranslateGlobal(Vector3 translation)
		{
			TransformGlobal(Matrix4x4.CreateTranslation(translation));
		}

		public void TranslateGlobal(float x, float y, float z)
		{
			TransformGlobal(Matrix4x4.CreateTranslation(x, y, z));
		}

		public void TranslateLocal(Vector3 translation)
		{
			TransformLocal(Matrix4x4.CreateTranslation(translation));
		}

		public void TranslateLocal(float x, float y, float z)
		{
			TransformLocal(Matrix4x4.CreateTranslation(x, y, z));
		}

		public Vector3 Transform(Vector3 position)
		{
			return Vector3.Transform(position, Matrix);
		}

		public void TransformGlobal(Transformation transform)
		{
			TransformGlobal(transform.Matrix);
		}
		public void TransformGlobal(Matrix4x4 transform)
		{
			Matrix *= transform;
		}

		public void TransformLocal(Transformation transform)
		{
			TransformLocal(transform.Matrix);
		}

		public void TransformLocal(Matrix4x4 transform)
		{
			Matrix = transform * Matrix;
		}
	}
}
