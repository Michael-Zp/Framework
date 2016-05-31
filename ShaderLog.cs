using System;
using System.Collections.Generic;

namespace Framework
{
	public class ShaderLogLine
	{
		public string Type;
		public int FileNumber;
		public int LineNumber;
		public string Message;
	}

	public class ShaderLog
	{
		public ShaderLog(string log)
		{
			//parse error log
			char[] newline = new char[] { '\n' };
			foreach (var line in log.Split(newline, StringSplitOptions.RemoveEmptyEntries))
			{
				var logLine = ParseLogLine(line);
				lines.Add(logLine);
			}
		}

		public IList<ShaderLogLine> Lines { get { return lines; } }

		private ShaderLogLine ParseLogLine(string line)
		{
			ShaderLogLine logLine = new ShaderLogLine();
			char[] colon = new char[] { ':' };
			var elements = line.Split(colon, 4);
			switch(elements.Length)
			{
				case 4:
					logLine.Type = elements[0];
					logLine.FileNumber = Parse(elements[1]);
					logLine.LineNumber = Parse(elements[2]);
					logLine.Message = elements[3];
					break;
				case 3:
					logLine.Type = elements[0];
					logLine.Message = elements[1] + ":" + elements[2];
					break;
				case 2:
					logLine.Type = elements[0];
					logLine.Message = elements[1];
					break;
				case 1:
					logLine.Message = elements[0];
					break;
				default:
					throw new ArgumentException(line);
			}
			return logLine;
		}

		private int Parse(string number)
		{
			int output;
			if (int.TryParse(number, out output))
			{
				return output;
			}
			else
			{
				return -1;
			}
		}

		private List<ShaderLogLine> lines = new List<ShaderLogLine>();
	}
}
