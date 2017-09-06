using Microsoft.Build.Evaluation;
using System;

namespace Tools
{
	public class ProjectResolveDMSDependencies
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine(nameof(ProjectResolveDMSDependencies) + " <sourceProjPath> <destProjPath>");
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
			proj.AddPackage("Zenseless", "0.1");
			proj.Save(destProjPath);
			proj.ProjectCollection.UnloadProject(proj);
		}
	}
}
