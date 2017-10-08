using Zenseless.Application;
using Zenseless.Base;
using System;
using System.IO;
using Zenseless.OpenGL;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			var visual = new MainVisual();
			app.ResourceManager.ShaderChanged += visual.ShaderChanged;

			LoadResources(app.ResourceManager);
			app.Render += visual.Render;
			app.GameWindow.AddMayaCameraEvents(visual.OrbitCamera);
			app.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			resourceManager.AddShader(MainVisual.ShaderName, dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);
			resourceManager.AddShader(MainVisual.ShaderDepthName, dir + "depthVert.glsl", dir + "depthFrag.glsl"
				, Resourcen.depthVert, Resourcen.depthFrag);
		}
	}
}