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
			LoadResources(app.ResourceManager);
			var controller = new Controller();
			var visual = new MainVisual();
			app.ResourceManager.ShaderChanged += visual.ShaderChanged;
			app.GameWindow.AddMayaCameraEvents(visual.OrbitCamera);

			var time = new GameTime();
			app.Render += visual.Render;
			app.Update += (t) => visual.Update(time.Seconds);
			app.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			resourceManager.Add(nameof(Resourcen.smoke), new ResourceTextureBitmap(Resourcen.smoke));
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			resourceManager.AddShader(VisualSmoke.ShaderName, dir + "smoke.vert", dir + "smoke.frag"
				, Resourcen.smoke_vert, Resourcen.smoke_frag);
			resourceManager.AddShader(VisualWaterfall.ShaderName, dir + "smoke.vert", dir + "smoke.frag"
				, Resourcen.smoke_vert, Resourcen.smoke_frag);
		}
	}
}