﻿using Zenseless.Application;
using System;
using Zenseless.OpenGL;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			Resources.LoadResources(app.ResourceManager);
			var visual = new MainVisual(app.RenderContext);
			app.GameWindow.AddMayaCameraEvents(visual.Camera);
			//app.ResourceManager.ShaderChanged += visual.ShaderChanged;
			app.Render += visual.Render;
			app.Resize += visual.Resize;
			app.Run();
		}
	}
}
