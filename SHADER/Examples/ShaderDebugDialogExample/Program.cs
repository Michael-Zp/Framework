using DMS.OpenGL;
using DMS.ShaderDebugging;
using DMS.System;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Example
{
	class MyWindow : IWindow
	{
		private ShaderFileDebugger shaderWatcher;

		public MyWindow()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);
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
			app.Run(new MyWindow());
		}
	}
}