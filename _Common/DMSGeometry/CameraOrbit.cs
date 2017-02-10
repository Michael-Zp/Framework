using System;
using System.Numerics;

namespace DMS.Geometry
{
	public class CameraOrbit
	{
		public CameraOrbit()
		{
			Aspect = 1;
			Distance = 1;
			FarClip = 1;
			FovY = 90;
			Heading = 0;
			NearClip = 0.1f;
			Target = Vector3.Zero;
			Tilt = 0;
		}

		public float Aspect { get; set; }
		public float Distance { get; set; }
		public float FarClip { get; set; }
		public float FovY { get; set; }
		public float Heading { get; set; }
		public float NearClip { get; set; }
		public Vector3 Target { get; set; }
		public float Tilt { get; set; }

		public Matrix4x4 CalcViewMatrix()
		{
			Distance = MathHelper.Clamp(Distance, NearClip, FarClip);
			var mtxDistance = Matrix4x4.Transpose(Matrix4x4.CreateTranslation(0, 0, -Distance));
			var mtxTilt = Matrix4x4.Transpose(Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(Tilt)));
			var mtxHeading = Matrix4x4.Transpose(Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(Heading)));
			var mtxTarget = Matrix4x4.Transpose(Matrix4x4.CreateTranslation(-Target));
			return mtxDistance * mtxTilt * mtxHeading * mtxTarget;
		}

		public Matrix4x4 CalcProjectionMatrix()
		{
			FovY = MathHelper.Clamp(FovY, 0.1f, 180);
			return Matrix4x4.Transpose(Matrix4x4.CreatePerspectiveFieldOfView(
				MathHelper.DegreesToRadians(FovY),
				Aspect, NearClip, FarClip));
		}

		public Matrix4x4 CalcMatrix()
		{
			return CalcProjectionMatrix() * CalcViewMatrix();
		}

		public Vector3 CalcPosition()
		{
			var view = CalcViewMatrix();
			Matrix4x4 inverse;
			if (!Matrix4x4.Invert(view, out inverse)) throw new ArithmeticException("Could not invert matrix");
			return new Vector3(inverse.M14, inverse.M24, inverse.M34);
		}
	}
}
