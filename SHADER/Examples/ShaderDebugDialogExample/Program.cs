using DMS.OpenGL;
using DMS.ShaderDebugging;
using DMS.System;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using OpenTK.Platform;

namespace Example
{
	class MyWindow : IWindow
	{
		private ShaderFileDebugger shaderWatcher;

		public MyWindow(IGameWindow gameWindow)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment, gameWindow);
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
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
		public static void Main()
		{
			var app = new ExampleApplication();
			var window = new MyWindow(app.GameWindow);
			app.Run(window);
		}
	}
}