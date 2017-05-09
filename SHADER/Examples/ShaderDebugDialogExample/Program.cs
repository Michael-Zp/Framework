using DMS.OpenGL;
using DMS.ShaderDebugging;
using DMS.Base;
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
			shaderWatcher.ShaderLoaded += ShaderWatcher_ShaderLoaded;
		}

		private void ShaderWatcher_ShaderLoaded()
		{
			//setup/update everything that depends on a loaded/changed shader
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
			}
			GL.Clear(ClearBufferMask.ColorBufferBit);

			var shader = shaderWatcher.Shader;
			if (ReferenceEquals(null, shader)) return;

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
			app.Run(new MyWindow());
		}
	}
}