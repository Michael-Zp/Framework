using System;
using System.Collections.Generic;

namespace MvcSokoban
{
	public class GameLogic
	{
		public GameLogic()
		{
			levels = Resourcen.levels.Split(new string[] { Environment.NewLine + Environment.NewLine, "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
			LoadLevel();
		}

		public ILevel GetLevel()
		{
			return levelLogic.GetLevel();
		}

		public void NextLevel()
		{
			++levelNr;
			if (levelNr >= levels.Length) --levelNr;
			LoadLevel();
		}

		public void ResetLevel()
		{
			LoadLevel();
		}

		public void Undo()
		{
			levelLogic.Undo();
		}

		public void Update(LevelLogic.Movement movement)
		{
			levelLogic.Update(movement);
			if (levelLogic.GetLevel().IsWon())
			{
				NextLevel();
			}
		}

		private uint levelNr = 1;
		private LevelLogic levelLogic;
		private string[] levels;

		private void LoadLevel()
		{
			//var levelString = Resourcen.ResourceManager.GetString("level" + levelNr.ToString());
			var level = LevelLoader.FromString(levels[levelNr]);
			if (ReferenceEquals(null, level)) return;
			levelLogic = new LevelLogic(level);
		}
	}
}
