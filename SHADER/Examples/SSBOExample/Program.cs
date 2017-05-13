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
			app.Run(new MainVisual());
		}
	}
}