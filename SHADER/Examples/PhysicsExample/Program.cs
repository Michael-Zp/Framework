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
			var visual = new MainVisual();
			app.Render += () => visual.Render(model.Bodies);
			app.Update += model.Update;
			app.GameWindow.ConnectEvents(visual.Camera);
			app.Run();
		}
	}
}