using Zenseless.Application;
using Zenseless.Base;
using System;
using System.IO;
using Zenseless.OpenGL;

namespace Example
{
	class Application
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MainVisual();
			window.GameWindow.AddMayaCameraEvents(visual.OrbitCamera);
			window.ResourceManager.ShaderChanged += visual.ShaderChanged;
			LoadResources(window.ResourceManager);

			var time = new GameTime();
			window.Render += () => visual.Render(time.DeltaTime);
			window.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			resourceManager.AddShader(MainVisual.ShaderName, dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);
		}
	}
}