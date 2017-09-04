using Microsoft.Build.Evaluation;
using System;
using System.IO;
using System.IO.Compression;

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
			var proj = new Project(args[0]);
			//proj.Items
			using (var zipFileStream = new FileStream(args[1], FileMode.Create))
			{
				var zip = new ZipArchive(zipFileStream, ZipArchiveMode.Create);
				var entry = zip.CreateEntry("__templateIcon.ico");
				//entry.Open().
				//zip.CreateEntryFromFile();
			}
		}
	}
}
