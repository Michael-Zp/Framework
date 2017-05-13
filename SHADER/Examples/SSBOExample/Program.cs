using DMS.Application;
using System;

namespace Example
{
	class Application
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var visual = new MainVisual();
			app.Render += visual.Render;
			app.Run();
		}
	}
}