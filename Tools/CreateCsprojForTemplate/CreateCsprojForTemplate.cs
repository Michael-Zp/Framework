using Microsoft.Build.Evaluation;
using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Tools
{
	public static class CreateCsprojForTemplate
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine($"{nameof(CreateCsprojForTemplate)} <sourceProjPath> <destProjPath>");
				return;
			}
			Execute(args[0], args[1]);
		}

		public static void Execute(string sourceProjPath, string destProjPath)
		{
			var proj = new Project(sourceProjPath);
			proj.RemoveProjRefs();
			proj.RemovePackRef("OpenTK");
			proj.RemovePackRef("NAudio");
			proj.AddPackage("Zenseless", GetLatestPackageVersion("Zenseless"));
			proj.Save(destProjPath);
			proj.ProjectCollection.UnloadProject(proj);
			var xmlProj = XDocument.Load(destProjPath);
			xmlProj.Set("ProjectGuid", "$guid1$");
			xmlProj.Replace("OutputPath", $"..{Path.DirectorySeparatorChar}", "");
			xmlProj.Save(destProjPath);
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

		//private static string GetPackageVersion()
		//{
		//	var xmlDoc = XDocument.Load(@"..\..\_common\Zenseless.nuspec"); //todo: resolve path when run from cmd; currently path error;
		//	var version = xmlDoc.Descendants("version").First();
		//	return version.ToString();
		//}

		private static string GetLatestPackageVersion(string packageID)
		{
			IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
			var version = repo.FindPackagesById(packageID).Max(p => p.Version);
			Console.WriteLine($"Using {packageID} version {version}");
			return version.ToString();
		}
	}
}
