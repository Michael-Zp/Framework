using DMS.Application;
using DMS.ShaderDebugging;
using DMS.Base;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Example
{
	class MyVisual
	{
		private const string ResourceVertexShader = "vertexShader";
		private const string ResourceFragmentShader = "fragmentShader";

		private ShaderFileDebugger shaderWatcher;

		public MyVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);
		}

		public void ResourceChanged(ResourceManager resourceManager, string resourceName)
		{
			//setup/update everything that depends on a loaded/changed resource
			//var vs = resourceManager.GetString(ResourceVertexShader);
			//var fs = resourceManager.GetString(ResourceFragmentShader);
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

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var visual = new MyVisual();
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			//app.ResourceManager.AddWatchedFileResource(ResourceVertexShader, dir + "vertex.glsl");
			//app.ResourceManager.AddWatchedFileResource(ResourceFragmentShader, dir + "fragment.glsl");
			//app.ResourceManager.ResourceChanged += window.ResourceChanged;
			app.Render += visual.Render;
			app.Run();
		}
	}
}