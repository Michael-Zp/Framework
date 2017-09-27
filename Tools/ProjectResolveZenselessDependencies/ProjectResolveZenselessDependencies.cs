using Microsoft.Build.Evaluation;
using NuGet;
using System;
using System.Linq;

namespace Tools
{
	public class ProjectResolveZenselessDependencies
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine(nameof(ProjectResolveZenselessDependencies) + " <sourceProjPath> <destProjPath>");
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
