using System.Numerics;

namespace Zenseless.Geometry
{
	public static class Box2dExtensions
	{
		public static Box2D CreateFromMinMax(float minX, float minY, float maxX, float maxY)
		{
			var rectangle = new Box2D(minX, minY, maxX - minX, maxY - minY);
			return rectangle;
		}

		public static Box2D CreateFromCenterSize(float centerX, float centerY, float sizeX, float sizeY)
		{
			var rectangle = new Box2D(0, 0, sizeX, sizeY)
			{
				CenterX = centerX,
				CenterY = centerY
			};
			return rectangle;
		}

		public static bool PushXRangeInside(this Box2D rectangleA, Box2D rectangleB)
		{
			if (rectangleA.SizeX > rectangleB.SizeX) return false;
			if (rectangleA.MinX < rectangleB.MinX)
			{
				rectangleA.MinX = rectangleB.MinX;
			}
			if (rectangleA.MaxX > rectangleB.MaxX)
			{
				rectangleA.MinX = rectangleB.MaxX - rectangleA.SizeX;
			}
			return true;
		}

		public static bool PushYRangeInside(this Box2D rectangleA, Box2D rectangleB)
		{
			if (rectangleA.SizeY > rectangleB.SizeY) return false;
			if (rectangleA.MinY < rectangleB.MinY)
			{
				rectangleA.MinY = rectangleB.MinY;
			}
			if (rectangleA.MaxY > rectangleB.MaxY)
			{
				rectangleA.MinY = rectangleB.MaxY - rectangleA.SizeY;
			}
			return true;
		}

		/// <summary>
		/// Calculates the AABR in the overlap
		/// Returns null if no intersection
		/// </summary>
		/// <param name="rectangleB"></param>
		/// <returns>AABR in the overlap</returns>
		public static Box2D Overlap(this Box2D rectangleA, Box2D rectangleB)
		{
			Box2D overlap = null;

			if (rectangleA.Intersects(rectangleB))
			{
				overlap = new Box2D(0.0f, 0.0f, 0.0f, 0.0f)
				{
					MinX = (rectangleA.MinX < rectangleB.MinX) ? rectangleB.MinX : rectangleA.MinX,
					MinY = (rectangleA.MinY < rectangleB.MinY) ? rectangleB.MinY : rectangleA.MinY
				};
				overlap.SizeX = (rectangleA.MaxX < rectangleB.MaxX) ? rectangleA.MaxX - overlap.MinX : rectangleB.MaxX - overlap.MinX;
				overlap.SizeY = (rectangleA.MaxY < rectangleB.MaxY) ? rectangleA.MaxY - overlap.MinY : rectangleB.MaxY - overlap.MinY;
			}

			return overlap;
		}

		public static void TransformCenter(this Box2D rectangle, Matrix3x2 M)
		{
			Vector2 center = new Vector2(rectangle.CenterX, rectangle.CenterY);
			var newCenter = Vector2.Transform(center, M);
			rectangle.CenterX = newCenter.X;
			rectangle.CenterY = newCenter.Y;
		}

		/// <summary>
		/// If an intersection with the frame occurs do the minimal translation to undo the overlap
		/// </summary>
		/// <param name="rectangleB">The AABR to check for intersect</param>
		public static void UndoOverlap(this Box2D rectangleA, Box2D rectangleB)
		{
			if (!rectangleA.Intersects(rectangleB)) return;

			Vector2[] directions = new Vector2[]
			{
				new Vector2(rectangleB.MaxX - rectangleA.MinX, 0), // push distance A in positive X-direction
				new Vector2(rectangleB.MinX - rectangleA.MaxX, 0), // push distance A in negative X-direction
				new Vector2(0, rectangleB.MaxY - rectangleA.MinY), // push distance A in positive Y-direction
				new Vector2(0, rectangleB.MinY - rectangleA.MaxY)  // push distance A in negative Y-direction
			};
			float[] pushDistSqrd = new float[4];
			for (int i = 0; i < 4; ++i)
			{
				pushDistSqrd[i] = directions[i].LengthSquared();
			}
			//find minimal positive overlap amount
			int minId = 0;
			for (int i = 1; i < 4; ++i)
			{
				minId = pushDistSqrd[i] < pushDistSqrd[minId] ? i : minId;
			}

			rectangleA.MinX += directions[minId].X;
			rectangleA.MinY += directions[minId].Y;
		}

		/// <summary>
		/// Create a box that is at least size <see cref="with"/> x <see cref="height"/>, but has aspect ratio <see cref="newWidth2heigth"/>
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="newWidth2heigth"></param>
		/// <returns></returns>
		public static Box2D CreateContainingBox(float width, float height, float newWidth2heigth)
		{
			float fWinAspect = width / height;
			bool isLandscape = newWidth2heigth < fWinAspect;
			float outputWidth = isLandscape ? width : height * newWidth2heigth;
			float outputHeight = isLandscape ? width / newWidth2heigth : height;
			var x = isLandscape ? 0f : (width - outputWidth) * .5f;
			var y = isLandscape ? (height - outputHeight) * .5f : 0f;
			return new Box2D(x, y, outputWidth, outputHeight);
		}

		public static Box2D MoveTo(this Box2D input, float minX, float minY)
		{
			return new Box2D(minX, minY, input.SizeX, input.SizeY);
		}
		public static Box2D MoveTo(this Box2D input, Vector2 min)
		{
			return new Box2D(min.X, min.Y, input.SizeX, input.SizeY);
		}

		public static Box2D Translate(this Box2D input, float tx, float ty)
		{
			return new Box2D(input.MinX + tx, input.MinY + ty, input.SizeX, input.SizeY);
		}
	}
}
