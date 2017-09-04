using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ProjToTemplate
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine(nameof(ProjToTemplate) + " <sourceProjPath> <destTemplateZipPath>");
				return;
			}
			var projFile = args[0];
			var proj = new Project(projFile);
			var dir = Path.GetDirectoryName(projFile) + Path.DirectorySeparatorChar;
			//proj.Items
			using (var zip = new ZipArchive(File.Create(args[1]), ZipArchiveMode.Create, false, System.Text.Encoding.UTF8))
			{
				//add files
				foreach (var file in Files(proj))
				{
					zip.CreateEntryFromFile(dir + file, file);
				}
				//add project file
				zip.CreateEntryFromFile(projFile, Path.GetFileName(projFile));
				//add icon file
				//var entry = zip.CreateEntry("__templateIcon.ico");
				//entry.Open().			}
			}
		}

		static IEnumerable<string> Files(Project proj)
		{
			return from item in proj.Items
						   where IsFile(item.ItemType)
						   select item.UnevaluatedInclude;
		}

		static bool IsFile(string itemType)
		{
			return ("Compile" == itemType) || ("None" == itemType) || ("EmbeddedResource" == itemType);
		}
	}
}
