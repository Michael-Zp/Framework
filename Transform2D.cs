using OpenTK;

namespace Framework
{
	public static class Transform2D
	{
		public static Matrix3 Rotate(float angle)
		{
			//Matrix3 does the rotation for row vectors 
			//-> for column vectors we need the transpose = the inverse for orthonormal
			return Matrix3.CreateRotationZ(-angle);
		}

		public static Matrix3 Translate(Vector2 t)
		{
			return new Matrix3( 1, 0, t.X,
								0, 1, t.Y,
								0, 0, 1);
		}

		public static Vector2 Transform(this Matrix3 M, Vector2 pos)
		{
			var pos3 = new Vector3(pos.X, pos.Y, 1);
			return new Vector2(Vector3.Dot(pos3, M.Row0), Vector3.Dot(pos3, M.Row1));
		}
	}
}
