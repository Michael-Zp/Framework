using DMS.Application;
using DMS.Base;
using DMS.Geometry;
using DMS.OpenGL;
using DMS.ShaderDebugging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Example
{
	public class MyVisual : IWindow
	{
		public MyVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);

			GL.Enable(EnableCap.DepthTest);
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
			geometry.Draw(instanceCount);
			shaderWatcher.Shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
		}

		private const int instanceCount = 10000;
		private ShaderFileDebugger shaderWatcher;
		private VAO geometry;

		private void UpdateGeometry(Shader shader)
		{
			Mesh mesh = Meshes.CreateSphere(0.03f, 2);
			geometry = VAOLoader.FromMesh(mesh, shader);

			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var instancePositions = new Vector3[instanceCount];
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			//todo students: add per instance attribute speed here
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyVisual());
		}
	}
}