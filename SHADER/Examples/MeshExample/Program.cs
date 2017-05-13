using DMS.Application;
using DMS.Base;
using DMS.Geometry;
using DMS.OpenGL;
using DMS.ShaderDebugging;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);

			//GL.Enable(EnableCap.DepthTest);
			//texDiffuse = TextureLoader.FromBitmap(Resourcen.capsule0);
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				UpdateGeometry(shaderWatcher.Shader);
			}

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderWatcher.Shader.Activate();
			//texDiffuse.Activate();
			geometry.Draw();
			//texDiffuse.Deactivate();
			shaderWatcher.Shader.Deactivate();
		}

		private ShaderFileDebugger shaderWatcher;
		private VAO geometry = new VAO();
		//private Texture texDiffuse;

		private void UpdateGeometry(Shader shader)
		{
			//load geometry
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry = VAOLoader.FromMesh(mesh, shader);
		}

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
