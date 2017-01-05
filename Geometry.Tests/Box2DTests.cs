using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geometry.Tests
{
	[TestClass()]
	public class Box2DTests
	{
		[TestMethod()]
		public void IntersectsTestNull()
		{
			var a = new Box2D(0, 0, 1, 1);
			var oldA = new Box2D(a);
			Assert.IsFalse(a.Intersects(null));
			Assert.AreEqual(a, oldA);
		}

		[TestMethod()]
		public void IntersectsTestNone()
		{
			var a = new Box2D(0, 0, 1, 1);
			var oldA = new Box2D(a);
			var b = new Box2D(5, 5, 1, 1);
			var oldB = new Box2D(b);
			Assert.IsFalse(a.Intersects(b));
			Assert.AreEqual(a, oldA);
			Assert.AreEqual(b, oldB);
		}

		[TestMethod()]
		public void IntersectsTestNone2()
		{
			var a = new Box2D(-4, -7, 10, 20);
			var b = new Box2D(6, -7, 10, 20);
			Assert.IsFalse(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsTestNone3()
		{
			var a = new Box2D(-4, -7, 10, 20);
			var b = new Box2D(-4, 13, 10, 20);
			Assert.IsFalse(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsTest()
		{
			var a = new Box2D(-4, -7, 1, 2);
			var b = new Box2D(a);
			Assert.IsTrue(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsTest2()
		{
			var a = new Box2D(-4, -7, 1, 2);
			var b = new Box2D(a);
			b.X += b.SizeX - 0.001f;
			Assert.IsTrue(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsTest3()
		{
			var a = new Box2D(-4, -7, 1, 2);
			var b = new Box2D(a);
			b.Y += b.SizeY - 0.001f;
			Assert.IsTrue(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}
	}
}