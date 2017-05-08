using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using DMS.ShaderDebugging;
using DMS.Base;
using System.IO;

namespace Example
{
	public class MyWindow : IWindow
	{
		public MyWindow()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);
			CreateMesh(shaderWatcher.Shader);

			//GL.Enable(EnableCap.DepthTest);
			//texDiffuse = TextureLoader.FromBitmap(Resourcen.capsule0);
		}

		public void Render()
		{
			var shader = shaderWatcher.Shader;
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				CreateMesh(shader);
			}

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			//texDiffuse.Activate();
			geometry.Draw();
			//texDiffuse.Deactivate();
			shader.Deactivate();
		}

		private void CreateMesh(Shader shader)
		{
			//load geometry
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry = VAOLoader.FromMesh(mesh, shader);
		}

		public void Update(float updatePeriod)
		{
		}

		private ShaderFileDebugger shaderWatcher;
		private VAO geometry = new VAO();
		//private Texture texDiffuse;

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}
	}
}
