using Microsoft.Build.Evaluation;
using System;

namespace ProjectResolveDMSDependencies
{
	class Program
	{
		static void Main(string[] args)
		{
			if(args.Length != 2)
			{
				Console.WriteLine(nameof(ProjectResolveDMSDependencies) + " <sourceProjPath> <destProjPath>");
				return;
			}
			var proj = new Project(args[0]);
			proj.RemoveProjRefs();
			proj.RemovePackRef("OpenTK");
			proj.RemovePackRef("NAudio");
			proj.AddPackage("DMS.Common", "0.2.2");
			proj.Save(args[1]);
		}


	}
}
