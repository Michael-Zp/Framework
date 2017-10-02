﻿using System;
using System.Collections.Generic;

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
			//for (int i = 1; i < 15; i++) won.Add(i);
			LoadLevel();
		}

		public ILevel GetLevelState()
		{
			return levelLogic.GetLevelState();
		}

		public bool HasLevelBeenWon(int levelNr)
		{
			return won.Contains(levelNr);
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
			if (levelLogic.GetLevelState().IsWon())
			{
				won.Add(LevelNr);
				++LevelNr;
			}
		}

		private LevelLogic levelLogic;
		private string[] levels;
		private HashSet<int> won = new HashSet<int>();
		private int levelNr;

		private void LoadLevel()
		{
			var level = LevelLoader.FromString(levels[LevelNr - 1]);
			if (ReferenceEquals(null, level)) return;
			levelLogic = new LevelLogic(level);
		}
	}
}