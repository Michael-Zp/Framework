using DMS.OpenGL;
using DMS.ShaderDebugging;
using DMS.Base;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using OpenTK.Platform;

namespace Example
{
	class MyWindow : IWindow
	{
		private ShaderDebugger shaderWatcher;

		public MyWindow(IGameWindow gameWindow)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment, gameWindow);
			shaderWatcher.ShaderLoaded += ShaderWatcher_ShaderLoaded;
		}

		private void ShaderWatcher_ShaderLoaded()
		{
			//setup geometry attributes when shader changes
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
			}
			var shader = shaderWatcher.Shader;
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shader.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var window = new MyWindow(app.GameWindow);
			app.Run(window);
		}
	}
}