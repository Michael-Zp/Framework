using DMS.Application;
using System;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			Resources.LoadResources(app.ResourceManager);
			var visual = new MainVisual();
			app.GameWindow.ConnectEvents(visual.Camera);
			//app.ResourceManager.ShaderChanged += visual.ShaderChanged;
			app.Render += visual.Render;
			app.Resize += visual.Resize;
			app.Run();
		}
	}
}
