using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Tools
{
	class PrepareExampleTemplates
	{
		static void Main(string[] args)
		{
			var path = Path.GetFullPath(args[0]);
			var xmlExamples = XDocument.Load(path);
			var dir = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;
			foreach(var xmlFromTo in xmlExamples.Descendants("FromTo"))
			{
				var source = xmlFromTo.Descendants("Source").First().Value;
				var destination = xmlFromTo.Descendants("Destination").First().Value;
				Execute(Path.GetFullPath(dir + source), Path.GetFullPath(dir + destination));
			}
		}

		static void Execute(string sourceProjPath, string destTemplateZipPath)
		{
			var output = $"Processing {sourceProjPath} and creating {destTemplateZipPath}";
			Log(output);
			var dir = Path.GetDirectoryName(sourceProjPath) + Path.DirectorySeparatorChar;
			var newProj = dir + Path.GetFileNameWithoutExtension(destTemplateZipPath) + ".csproj";
			try
			{
				if (newProj == sourceProjPath) throw new ArgumentException($"{nameof(sourceProjPath)} and {nameof(destTemplateZipPath)} are required to have different file names");
				ProjectResolveZenselessDependencies.Execute(sourceProjPath, newProj);
				ProjToTemplate.Execute(newProj, destTemplateZipPath);
				File.Delete(newProj);
			}
			catch (Exception e)
			{
				Log(e.Message);
			}
		}

		private static void Log(string output)
		{
			Console.WriteLine(output);
			Debug.WriteLine(output);
		}
	}
}
