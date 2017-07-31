using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MvcSokoban
{
	public class LevelLogic
	{
		public enum Movement { NONE = 0, UP, DOWN, LEFT, RIGHT };

		private Point playerPos;
		private List<Level> levelStates = new List<Level>();

		public LevelLogic(Level level)
		{
			levelStates.Add(level);
			Point? playerPos = level.FindPlayerPos();
			if (playerPos.HasValue)
			{
				this.playerPos = playerPos.Value;
			}
		}

		public ILevel GetLevel() { return levelStates.Last(); }

		public void Undo()
		{
			if (levelStates.Count > 1) levelStates.RemoveAt(levelStates.Count - 1);
		}

		public void Update(Movement movement)
		{
			UpdateMovables(movement);
		}

		private void UpdateMovables(Movement movement)
		{
			Point newPlayerPos = playerPos;
			newPlayerPos = CalcNewPosition(newPlayerPos, movement);
			ElementType type = GetLevel().GetElement(newPlayerPos.X, newPlayerPos.Y);
			if (ElementType.Wall == type) return;
			levelStates.Add(levelStates.Last().Copy());
			if (ElementType.Box == type ||ElementType.BoxOnGoal == type)
			{
				//box will be moved
				Point newBoxPos = CalcNewPosition(newPlayerPos, movement);
				ElementType type2 = GetLevel().GetElement(newBoxPos.X, newBoxPos.Y);
				//is new box position invalid
				if (ElementType.Floor != type2 && ElementType.Goal != type2) return;
				//moving box
				levelStates.Last().MoveBox(newPlayerPos, newBoxPos);
			}
			Point oldPlayerPos = playerPos;
			playerPos = newPlayerPos;
			levelStates.Last().MovePlayer(oldPlayerPos, playerPos);
		}

		private static Point CalcNewPosition(Point pos, Movement movement)
		{
			Point newPos = pos;
			switch (movement)
			{
				case Movement.DOWN: newPos = new Point(pos.X, pos.Y - 1); break;
				case Movement.UP: newPos = new Point(pos.X, pos.Y + 1); break;
				case Movement.LEFT: newPos = new Point(pos.X - 1, pos.Y); break;
				case Movement.RIGHT: newPos = new Point(pos.X + 1, pos.Y); break;
			}

			return newPos;
		}
	}
}
