using System.Collections.Generic;
using Zenseless.Base;
using Zenseless.Geometry;

namespace Example
{
	public class Model
	{
		public IEnumerable<IReadOnlyBox2D> Shapes => new[] { player, obstacle };

		public void Update(float movementX, float updatePeriod)
		{
			//player movement
			player.MinX += movementX * updatePeriod;

			//todo student: let the player also move up and down
			//todo student:Limit player movements to window

			//no intersection -> move obstacle
			if (!obstacle.Intersects(player))
			{
				obstacle.MinY -= 0.5f * updatePeriod;
			}

			if (obstacle.MaxY < -1)
			{
				obstacle.MinY = 1;
			}
		}

		private Box2D obstacle = new Box2D(-0.2f, 1, 0.4f, 0.4f);
		private Box2D player = new Box2D(0.0f, -0.95f, 0.2f, 0.2f);
		private ITime time;

		public Model(ITime time)
		{
			this.time = time;
		}
	}
}
