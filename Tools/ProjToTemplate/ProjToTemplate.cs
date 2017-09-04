using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Tools
{
	public class ProjToTemplate
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine(nameof(ProjToTemplate) + " <sourceProjPath> <destTemplateZipPath>");
				return;
			}
			Execute(args[0], args[1]);
		}

		public static void Execute(string sourceProjPath, string destTemplateZipPath)
		{
			using (var zip = new ZipArchive(File.Create(destTemplateZipPath), ZipArchiveMode.Create, false, Encoding.UTF8))
			{
				//add icon file
				using (var iconEntry = zip.CreateEntry("__templateIcon.ico").Open())
				{
					ResTemplate.__TemplateIcon.Save(iconEntry);
				}

				//add project file
				var projFileName = Path.GetFileName(sourceProjPath);
				zip.CreateEntryFromFile(sourceProjPath, projFileName);

				//update template manifest
				var xml = XDocument.Parse(Encoding.UTF8.GetString(ResTemplate.MyTemplate));
				var ns = xml.Root.Name.Namespace;
				xml.Descendants(ns + "Name").First().SetValue(Path.GetFileNameWithoutExtension(projFileName));
				xml.Descendants(ns + "Description").First().SetValue("Example for lecture CG");
				var xmlProj = xml.Descendants(ns + "Project").First();
				xmlProj.Add(new XAttribute("File", projFileName));

				//add files
				var dir = Path.GetDirectoryName(sourceProjPath) + Path.DirectorySeparatorChar;
				var proj = new Project(sourceProjPath);
				foreach (var file in Files(proj))
				{
					zip.CreateEntryFromFile(dir + file, file);
					xmlProj.Add(new XElement(ns + "ProjectItem", file));
				}
				//add vstemplate
				using (var entryVsTemplate = zip.CreateEntry("MyTemplate.vstemplate").Open())
				{
					xml.Save(entryVsTemplate);
				}
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
