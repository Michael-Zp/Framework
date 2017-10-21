using Microsoft.Build.Evaluation;
using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Tools
{
	public static class SwitchProjRefToPackage
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine($"{nameof(SwitchProjRefToPackage)} <sourceProjPath> <destProjPath>");
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
		}

		private static string GetLatestPackageVersion(string packageID)
		{
			try
			{
				return GetLatestPackageVersionFromNuget();
			}
			catch
			{
				//todo: resolve path when run from cmd; currently path error;
				return GetPackageVersionFromNuspec(@"..\..\_common\Zenseless.nuspec");
			}
		}

		private static string GetLatestPackageVersionFromNuget(string packageID)
		{
			IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
			var version = repo.FindPackagesById(packageID).Max(p => p.Version);
			Console.WriteLine($"Using {packageID} version {version}");
			return version.ToString();
		}

		private static string GetPackageVersionFromNuspec(string nuspecFilePath)
		{
			var xmlDoc = XDocument.Load(nuspecFilePath);
			var version = xmlDoc.Descendants("version").First();
			return version.ToString();
		}
	}
}
