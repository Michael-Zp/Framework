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
	public static class ProjToTemplate
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
				using (var entryProj = zip.CreateEntry(projFileName).Open())
				{
					var xmlProj = PrepareProjectForTemplate(sourceProjPath);
					xmlProj.Save(entryProj);
				}

				//add project files
				var dir = Path.GetDirectoryName(sourceProjPath) + Path.DirectorySeparatorChar;
				var projFiles = Files(sourceProjPath);
				foreach (var file in projFiles)
				{
					zip.CreateEntryFromFile(dir + file, file);
				}

				//add vs template manifest
				using (var entryVsTemplate = zip.CreateEntry("MyTemplate.vstemplate").Open())
				{
					var xmlDoc = CreateManifest(projFileName, projFiles);
					xmlDoc.Save(entryVsTemplate);
				}
			}
		}

		static IEnumerable<string> Files(string projectFilePath)
		{
			var proj = new Project(projectFilePath);
			return from item in proj.Items
						   where IsFile(item.ItemType)
						   select item.UnevaluatedInclude;
		}

		static bool IsFile(string itemType)
		{
			return ("Compile" == itemType) || ("None" == itemType) || ("EmbeddedResource" == itemType);
		}

		static XDocument CreateManifest(string projFileName, IEnumerable<string> projFiles)
		{
			var xmlDoc = XDocument.Parse(Encoding.UTF8.GetString(ResTemplate.MyTemplate));
			var ns = xmlDoc.Root.Name.Namespace;
			xmlDoc.Descendants(ns + "Name").First().SetValue(Path.GetFileNameWithoutExtension(projFileName));
			xmlDoc.Descendants(ns + "Description").First().SetValue("Exercise for lecture CG");
			var xmlProjNode = xmlDoc.Descendants(ns + "Project").First();
			xmlProjNode.Add(new XAttribute("File", projFileName));

			//add files
			foreach (var file in projFiles)
			{
				xmlProjNode.Add(new XElement(ns + "ProjectItem", file));
			}
			return xmlDoc;
		}

		static XDocument PrepareProjectForTemplate(string projectFilePath)
		{
			var xmlProj = XDocument.Load(projectFilePath);
			xmlProj.Set("ProjectGuid", "$guid1$");
			xmlProj.Replace("OutputPath", $"..{Path.DirectorySeparatorChar}", "");
			return xmlProj;
		}

		private static void Replace(this XDocument xmlProj, string element, string input, string output)
		{
			var ns = xmlProj.Root.Name.Namespace;
			foreach (var xmlOutputPath in xmlProj.Descendants(ns + element))
			{
				var newValue = xmlOutputPath.Value.Replace(input, output);
				xmlOutputPath.SetValue(newValue);
			}
		}

		private static void Set(this XDocument xmlProj, string element, string value)
		{
			var ns = xmlProj.Root.Name.Namespace;
			foreach (var xmlOutputPath in xmlProj.Descendants(ns + element))
			{
				xmlOutputPath.SetValue(value);
			}
		}
	}
}
