using Zenseless.Application;
using Zenseless.Base;
using System;
using System.IO;

namespace Example
{
	class Application
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			var visual = new MainVisual();
			app.ResourceManager.ShaderChanged += visual.ShaderChanged;
			LoadResources(app.ResourceManager);
			var time = new GameTime();
			app.Render += () => visual.Render(time.DeltaTime);
			app.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			resourceManager.AddShader(MainVisual.ShaderName, dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);
		}
	}
}