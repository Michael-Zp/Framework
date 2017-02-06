using Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace LevelEditor
{
	[Serializable]
	public class Element
	{
		public string Name { get; set; }
	}

	[Serializable]
	public class ColliderCircle : Element
	{
		public ColliderCircle(Circle bounds)
		{
			Bounds = bounds;
		}

		public Circle Bounds { get; set; }
	}

	[Serializable]
	public class Sprite : Element
	{
		public Sprite(Box2D renderBounds)
		{
			RenderBounds = renderBounds;
		}

		public Box2D RenderBounds { get; set; }
		public string TextureName { get; set; }
		public Bitmap Texture { get; set; }
	}

	[Serializable]
	public class Level
	{
		public Box2D Bounds = new Box2D(Box2D.BOX01);

		public void Add(int layer, Sprite sprite)
		{
			if(!layers.ContainsKey(layer))
			{
				layers.Add(layer, new List<Sprite>());
			}
			layers[layer].Add(sprite);
		}

		public void Add(ColliderCircle collider)
		{
			colliders.Add(collider);
		}

		public Dictionary<int, List<Sprite>> layers = new Dictionary<int, List<Sprite>>();
		public List<ColliderCircle> colliders = new List<ColliderCircle>();

		public void MovePlayer(float deltaX, float deltaY)
		{
			var player = layers[2].First();
			player.RenderBounds.X += deltaX;
			player.RenderBounds.Y += deltaY;
			var circlePlayer = CircleExtensions.CreateFromBox(player.RenderBounds);
			var collisions = new List<Circle>();
			foreach(var collider in colliders)
			{
				var circleCollider = collider.Bounds;
				if (circleCollider.Intersects(circlePlayer))
				{
					collisions.Add(circleCollider);
				}
			}
			if (1 < collisions.Count)
			{
				//more than one collision -> no movement
				player.RenderBounds.X -= deltaX;
				player.RenderBounds.Y -= deltaY;
			}
			else if (1 == collisions.Count)
			{
				//try handling collision
				circlePlayer.UndoOverlap(collisions.First());
				player.RenderBounds.CenterX = circlePlayer.CenterX;
				player.RenderBounds.CenterY = circlePlayer.CenterY;
			}
			player.RenderBounds.PushXRangeInside(Bounds);
			player.RenderBounds.PushYRangeInside(Bounds);
		}
	}
}
