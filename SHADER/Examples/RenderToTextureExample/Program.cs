using DMS.Application;
using DMS.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using System.Text;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var visual = new MainVisual();
			app.ResourceManager.ShaderChanged += visual.ShaderChanged;
			LoadResources(app.ResourceManager);

			Stopwatch globalTime = new Stopwatch();
			bool doPostProcessing = false;

			app.Render += () =>
			{
				float time = (float)globalTime.Elapsed.TotalSeconds;
				if (doPostProcessing)
				{
					visual.DrawWithPostProcessing(time);
				}
				else
				{
					visual.Draw();
				}
			};

			app.Update += (t) => doPostProcessing = !Keyboard.GetState()[Key.Space];
			app.Resize += visual.Resize;
			app.GameWindow.ConnectEvents(visual.OrbitCamera);

			globalTime.Start();
			app.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			resourceManager.AddShader(MainVisual.ShaderPostProcessName, TextureToFrameBuffer.VertexShaderScreenQuad, Encoding.UTF8.GetString(Resources.Swirl));
			resourceManager.AddShader(MainVisual.ShaderName, Encoding.UTF8.GetString(Resources.vertex), Encoding.UTF8.GetString(Resources.fragment));
		}
	}
}