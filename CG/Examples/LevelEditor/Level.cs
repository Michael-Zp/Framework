using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelEditor
{
	[Serializable]
	public class Sprite
	{
		public Sprite(float minX, float maxX, float minY, float maxY)
		{
			Bounds = Box2D.CreateFromMinMax(minX, minY, maxX, maxY);
		}

		public Box2D Bounds { get; set; }
		public string Name { get; set; }
		public string TextureName { get; set; }
	}

	[Serializable]
	public class Level
	{
		public void Add(int layer, Sprite sprite)
		{
			if(!layers.ContainsKey(layer))
			{
				layers.Add(layer, new List<Sprite>());
			}
			layers[layer].Add(sprite);
		}

		public Dictionary<int, List<Sprite>> layers = new Dictionary<int, List<Sprite>>();

		public void MovePlayer(float deltaX, float deltaY)
		{
			var player = layers[2].First();
			player.Bounds.X += deltaX;
			player.Bounds.Y += deltaY;
			foreach(var collider in layers[1])
			{
				if(collider.Bounds.Intersects(player.Bounds))
				{
					player.Bounds.X -= deltaX;
					player.Bounds.Y -= deltaY;
					break;
				}
			}
			player.Bounds.PushXRangeInside(windowBorders);
			player.Bounds.PushYRangeInside(windowBorders);
		}

		private Box2D windowBorders = Box2D.BOX01;
	}
}
