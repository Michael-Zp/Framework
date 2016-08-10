using System.Collections.Generic;
using System.IO;

namespace MvcSokoban
{
	public class LevelLoader
	{
		public static Level FromFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException("Could not find level file '" + fileName + "'");
			}
			var sLevel = new List<string>();
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (sr.Peek() >= 0)
				{
					sLevel.Add(sr.ReadLine());
				}
			}
			if (0 == sLevel.Count) return null;
			int width = 0;
			foreach (string sLine in sLevel)
			{
				//find longest line
				if (sLine.Length > width)
				{
					width = sLine.Length;
				}
			}
			//use line count and the longest line as level dimensions
			Level level = new Level(width, sLevel.Count);
			int y = level.Height - 1;
			foreach (string sLine in sLevel)
			{
				int x = 0;
				//each character is a grid element
				foreach (char symbol in sLine)
				{
					ElementType type = ElementType.Floor;
					switch (symbol)
					{
						case '#': type = ElementType.Wall; break;
						case '-': type = ElementType.Floor; break;
						case '@': type = ElementType.Man; break;
						case '$': type = ElementType.Box; break;
						case '.': type = ElementType.Goal; break;
						case '*': type = ElementType.BoaxOnGoal; break;
						case '+': type = ElementType.ManOnGoal; break;
					};
					level.SetElement(x, y, type);
					++x;
				}
				--y;
			}
			return level;
		}
	}
}
