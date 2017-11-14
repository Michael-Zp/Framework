using Zenseless.Application;
using Zenseless.Base;
using OpenTK.Input;
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

			var globalTime = new GameTime();
			bool doPostProcessing = false;

			app.Render += () =>
			{
				if (doPostProcessing)
				{
					visual.DrawWithPostProcessing(globalTime.AbsoluteTime);
				}
				else
				{
					visual.Draw();
				}
			};

			app.Update += (t) => doPostProcessing = !Keyboard.GetState()[Key.Space];
			app.Resize += visual.Resize;
			app.GameWindow.AddMayaCameraEvents(visual.OrbitCamera);

			app.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			resourceManager.AddShader(MainVisual.ShaderName, dir + "vertex.vert", dir + "fragment.frag", Resources.vertex, Resources.fragment);
			//change fragment shader to switch between post-processing methods
			resourceManager.AddShader(MainVisual.ShaderPostProcessName, dir + "vertexPostProcess.vert", dir + "Grayscale.glsl", null, null);
		}
	}
}