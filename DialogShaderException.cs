using System;
using System.Collections.Generic;
using System.Drawing;

namespace Framework
{
	public class DialogShaderException
	{
		public static void Show(ShaderException e)
		{
			var form = new FormShaderError();
			

			var log = new ShaderLog(e.Log);
			//var errors = new List<string>();
			//form.UpdateErrors(log.Lines);


			var rtf = form.richTextBox;
			foreach (var logLine in log.Lines)
			{
				rtf.AppendText("Line " + logLine.LineNumber + " " + logLine.Message + Environment.NewLine);
			}
			//form.listBox.DataSource = errors;

			var font = rtf.Font;
			var errorFont = new Font(font, FontStyle.Strikeout);
			char[] newline = new char[] { '\n' };
			var sourceLines = e.ShaderCode.Split(newline);
			//foreach (var sourceLine in sourceLines)
			//{
			//	rtf.AppendText(sourceLine);
			//}
			//	rtf.AppendText("Line " + logLine.LineNumber + " " + logLine.Message + Environment.NewLine);
			//	if ("ERROR" == logLine.Type)
			//	{
			//		rtf.SelectionColor = Color.Red;
			//	}
			//}
			//for (int i = 0; i < sourceLines.Length; ++i)
			//{
			//	ShaderLogLine logLine;
			//	if (log.Lines.TryGetValue(i + 1, out logLine))
			//	{
			//		rtf.SelectionColor = Color.Red;
			//		rtf.SelectionFont = errorFont;
			//	}
			//	else
			//	{
			//		rtf.SelectionColor = Color.Gray;
			//	}
			//	rtf.AppendText(sourceLines[i]);
			//}

			form.ShowDialog();
		}
	}
}
