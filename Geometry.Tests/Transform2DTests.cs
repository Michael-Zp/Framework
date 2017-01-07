using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Geometry.Tests
{
	[TestClass()]
	public class Transform2DTests
	{
		[TestMethod()]
		public void RotateAroundOriginTest90()
		{
			var a = Vector2.UnitX;
			var m = Transform2D.CreateRotationAroundOrigin(0.25f * MathHelper.TWO_PI);
			var expectedA = Vector2.UnitY;
			Assert.AreEqual(expectedA, Vector2.Transform(a, m));
		}

		[TestMethod()]
		public void RotateAroundOriginTest180()
		{
			var a = Vector2.UnitX;
			var m = Transform2D.CreateRotationAroundOrigin(0.5f * MathHelper.TWO_PI);
			var expectedA = -a;
			Assert.AreEqual(expectedA, Vector2.Transform(a, m));
		}

		[TestMethod()]
		public void RotateAroundOriginTestIdentity()
		{
			var m = Transform2D.CreateRotationAroundOrigin(MathHelper.TWO_PI);
			var expectedM = Matrix3x2.Identity;
			Assert.AreEqual(expectedM, m);
		}

		[TestMethod()]
		public void RotateAroundOriginTestIdentity2()
		{
			var m = Transform2D.CreateRotationAroundOrigin(0);
			var expectedM = Matrix3x2.Identity;
			Assert.AreEqual(expectedM, m);
		}

		[TestMethod()]
		public void RotateAroundTest()
		{
			var a = Vector2.Zero;
			var m = Transform2D.CreateRotationAround(-1, -1, 0.25f * MathHelper.TWO_PI);
			var expectedA = -2 * Vector2.UnitX;
			Assert.AreEqual(expectedA, Vector2.Transform(a, m));
		}

		[TestMethod()]
		public void ScaleAroundOriginTest()
		{
			var a = new Vector2(-3, 2);
			var m = Transform2D.CreateScaleAroundOrigin(2, -3);
			var expectedA = new Vector2(-6, -6);
			Assert.AreEqual(expectedA, Vector2.Transform(a, m));
		}

		[TestMethod()]
		public void ScaleAroundTest()
		{
			var a = new Vector2(1, 1);
			var m = Transform2D.CreateScaleAround(-1, -1, 2, 3);
			var expectedA = new Vector2(3, 5);
			Assert.AreEqual(expectedA, Vector2.Transform(a, m));
		}
	}
}