﻿using Zenseless.Application;
using Zenseless.Base;
using System;
using System.IO;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			//app.IsRecording = true;
			var visual = new MainVisual();
			window.ResourceManager.ShaderChanged += visual.ShaderChanged;
			LoadResources(window.ResourceManager);

			var time = new GameTime();
			window.Render += visual.Render;
			window.Update += (dt) => visual.Update(time.AbsoluteTime);
			window.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			resourceManager.AddShader(MainVisual.ShaderName, dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);
		}
	}
}
