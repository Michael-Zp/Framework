using System.IO;
using System.Runtime.CompilerServices;

namespace Tools
{
	class PrepareExampleTemplates
	{
		public static string GetSourceFilePath([CallerFilePath] string doNotAssignCallerFilePath = "")
		{
			return doNotAssignCallerFilePath;
		}

		static void Main(string[] args)
		{
			var thisDir = Path.GetDirectoryName(GetSourceFilePath()) + "/";
			var srcDir = thisDir + "../../CG/Examples/";
			var dstDir = thisDir + "../Zenseless.VSX/ProjectTemplates/";
			Execute(srcDir + "TextureExample/TextureExample.csproj", dstDir + "1. Example.zip");
		}

		static void Execute(string sourceProjPath, string destTemplateZipPath)
		{
			var dir = Path.GetDirectoryName(sourceProjPath) + "/";
			var newProj = dir + Path.GetFileNameWithoutExtension(destTemplateZipPath) + ".csproj";
			ProjectResolveDMSDependencies.Execute(sourceProjPath, newProj);
			ProjToTemplate.Execute(newProj, destTemplateZipPath);
			File.Delete(newProj);
		}
	}
}
