using DMS.Application;
using DMS.Base;
using System;
using System.IO;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var model = new Model();
			var visual = new MainVisual();
			window.ResourceManager.ShaderChanged += visual.ShaderChanged;
			LoadResources(window.ResourceManager);
			window.Render += () => visual.Render(model.Bodies);
			window.Update += model.Update;
			window.GameWindow.ConnectEvents(visual.Camera);
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