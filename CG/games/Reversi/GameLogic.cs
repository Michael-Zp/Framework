using System.Drawing;

namespace Reversi
{
	public class GameLogic
	{
		public GameLogic()
		{
			for (int x = 0; x < 8; ++x)
			{
				for (int y = 0; y < 8; ++y)
				{
					grid[x, y] = FieldType.EMPTY;
				}
			}
			grid[3, 3] = FieldType.BLACK;
			grid[3, 4] = FieldType.WHITE;
			grid[4, 3] = FieldType.WHITE;
			grid[4, 4] = FieldType.BLACK;
			grid.LastMove = new Point(3, 4);
		}

		public IGameState GameState {  get { return grid; } }

		public void Move(Point point)
		{
			var x = point.X;
			var y = point.Y;
			if (x < 0 || 7 < x) return;
			if (y < 0 || 7 < y) return;
			if (FieldType.EMPTY != grid[x, y]) return;
			var color = whiteMoves ? FieldType.WHITE : FieldType.BLACK;
			grid[x, y] = color;
			grid.LastMove = new Point(x, y);
			for (int dirX = -1; dirX <= 1; ++dirX)
			{
				for (int dirY = -1; dirY <= 1; ++dirY)
				{
					if (0 == dirX && 0 == dirY) continue;
					Reverse(x, y, color, dirX, dirY);
				}
			}
			whiteMoves = !whiteMoves;
		}

		private void Reverse(int startX, int startY, FieldType fillColor, int dirX, int dirY)
		{
			var otherColor = FieldType.BLACK == fillColor ? FieldType.WHITE : FieldType.BLACK;
			//search how many to reverse
			for (int i = 1; true; ++i)
			{
				//go one step into direction
				int x = startX + i * dirX;
				int y = startY + i * dirY;
				//check out of bounds
				if (x < 0 || 7 < x) return;
				if (y < 0 || 7 < y) return;
				if (otherColor != grid[x, y])
				{
					if (fillColor == grid[x, y])
					{
						for (int j = 1; j < i; ++j)
						{
							//reverse
							int reverseX = startX + j * dirX;
							int reverseY = startY + j * dirY;
							grid[reverseX, reverseY] = fillColor;
						}
					}
					return;
				}
			}
		}

		private GameState grid = new GameState();
		private bool whiteMoves = false;

	}
}
