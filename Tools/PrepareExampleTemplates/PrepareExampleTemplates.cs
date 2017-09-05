using System;
using System.Diagnostics;
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
			var examples = new string[] {
				"MinimalExample", "CollisionExample", "TextureExample", "TextureWrapExample",
				"TextureCoordExample", "TextureFontExample", "TextureAnimExample"};
			int i = 1;
			foreach(var example in examples)
			{
				Execute(srcDir + example + "/" + example + ".csproj", dstDir + "CG " + i + ". Example.zip");
				i++;
			}
		}

		static void Execute(string sourceProjPath, string destTemplateZipPath)
		{
			Debug.WriteLine($"Processing {sourceProjPath} and creating {destTemplateZipPath}");
			var dir = Path.GetDirectoryName(sourceProjPath) + "/";
			var newProj = dir + Path.GetFileNameWithoutExtension(destTemplateZipPath) + ".csproj";
			ProjectResolveDMSDependencies.Execute(sourceProjPath, newProj);
			ProjToTemplate.Execute(newProj, destTemplateZipPath);
			File.Delete(newProj);
		}
	}
}
