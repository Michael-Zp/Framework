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

		public void Reset()
		{
			Matrix = Matrix4x4.Identity;
		}

		/// <summary>
		/// Rotate Transform
		/// </summary>
		/// <param name="degrees"></param>
		public void RotateX(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(degrees)));
		}

		public void RotateY(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(degrees)));
		}

		public void RotateZ(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationZ(MathHelper.DegreesToRadians(degrees)));
		}

		public void Scale(Vector3 scales)
		{
			TransformGlobal(Matrix4x4.CreateScale(scales));
		}

		public void Scale(float x, float y, float z)
		{
			TransformGlobal(Matrix4x4.CreateScale(x, y, z));
		}

		public void Translate(Vector3 translation)
		{
			TransformGlobal(Matrix4x4.CreateTranslation(translation));
		}

		public void Translate(float x, float y, float z)
		{
			TransformGlobal(Matrix4x4.CreateTranslation(x, y, z));
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

		public Matrix4x4 Matrix { get; private set; }
	}
}
