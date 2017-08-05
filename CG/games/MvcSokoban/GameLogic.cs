using System;

namespace MvcSokoban
{
	[Serializable]
	public class GameLogic
	{
		public int LevelNr
		{
			get { return levelNr; }
			set
			{
				if (value == levelNr) return;
				levelNr = Math.Min(value, levels.Length);
				levelNr = Math.Max(levelNr, 1);
				LoadLevel();
			}
		}
		public int Moves { get { return levelLogic.Moves; } }

		public GameLogic()
		{
			levelNr = 1;
			levels = Resourcen.levels.Split(new string[] { Environment.NewLine + Environment.NewLine, "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
			LoadLevel();
		}

		public ILevel GetLevel()
		{
			return levelLogic.GetLevel();
		}

		public void ResetLevel()
		{
			LoadLevel();
		}

		public void Undo()
		{
			levelLogic.Undo();
		}

		public void Update(Movement movement)
		{
			levelLogic.Update(movement);
			if (levelLogic.GetLevel().IsWon())
			{
				++LevelNr;
			}
		}

		private LevelLogic levelLogic;
		private string[] levels;
		private int levelNr;

		private void LoadLevel()
		{
			var level = LevelLoader.FromString(levels[LevelNr - 1]);
			if (ReferenceEquals(null, level)) return;
			levelLogic = new LevelLogic(level);
		}
	}
}