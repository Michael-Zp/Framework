using DMS.Application;
using System;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var model = new Model();
			app.Update += model.Update;
			var visual = new MainVisual();
			app.Render += () => visual.Render(model.Bodies);
			app.GameWindow.ConnectMouseEvents(visual.Camera);
			app.Run();
		}
	}
}