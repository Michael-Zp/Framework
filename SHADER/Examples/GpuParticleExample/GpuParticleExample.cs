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
			var window = new ExampleWindow();
			var visual = new MainVisual();
			window.GameWindow.ConnectEvents(visual.OrbitCamera);
			window.ResourceManager.ShaderChanged += visual.ShaderChanged;
			LoadResources(window.ResourceManager);
			window.Render += () => window.GameWindow.Title = Math.Round(visual.Render()).ToString() + "msec";
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