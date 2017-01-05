using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geometry.Tests
{
	[TestClass()]
	public class Box2dExtensionsTests
	{
		[TestMethod()]
		public void UndoOverlapTestNoOverlap()
		{
			var a = new Box2D(-1, -3, 1, 2);
			var b = new Box2D(0, -1, 1, 1);
			var oldA = new Box2D(a);
			a.UndoOverlap(b);
			Assert.AreEqual(a, oldA);
		}

		[TestMethod()]
		public void UndoOverlapTest2()
		{
			var aX = -1f;
			var a = new Box2D(aX + 0.1f, 0, 2, 2);
			var b = new Box2D(1, 0, 2, 2);
			a.UndoOverlap(b);
			Assert.AreEqual(a.X, aX);
		}

		[TestMethod()]
		public void UndoOverlapTest3()
		{
			var a = new Box2D(0, 0, 0, 0);
			var b = new Box2D(a);
			a.UndoOverlap(b);
			Assert.AreEqual(a, b);
		}

		[TestMethod()]
		public void UndoOverlapTest4()
		{
			var a = new Box2D(-1, -1, 2, 3);
			var b = new Box2D(a);
			var newA = new Box2D(a);
			newA.X += 2f;
			a.X += 0.01f; //to make it unambiguous in which direction to push
			a.UndoOverlap(b);
			Assert.AreEqual(a, newA);
		}

		[TestMethod()]
		public void UndoOverlapTest5()
		{
			var a = new Box2D(-1, -1, 3, 2);
			var b = new Box2D(a);
			var newA = new Box2D(a);
			newA.Y += 2f;
			a.Y += 0.01f; //to make it unambiguous in which direction to push
			a.UndoOverlap(b);
			Assert.AreEqual(a, newA);
		}
	}
}