using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using DMS.ShaderDebugging;
using System.IO;
using DMS.Base;

namespace Example
{
	public class MyWindow : IWindow
	{
		public MyWindow()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);
			var shader = shaderWatcher.Shader;
			geometry = CreateMesh(shader);

			GL.ClearColor(1, 1, 1, 1);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			timeSource.Start();
		}

		public void Render()
		{
			var shader = shaderWatcher.Shader;
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				geometry = CreateMesh(shader);
			}

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			Matrix4 camera = Matrix4.CreateScale(1, 1, -1);
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref camera);
			geometry.Draw(instanceTransforms.Length);
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
			var time = (float)timeSource.Elapsed.TotalSeconds;
			//store matrices as per instance attributes
			//Matrix4 transforms are row-major -> transforms are written T1*T2*...
			for (int i = 0; i < instanceTransforms.Length; ++i)
			{
				instanceTransforms[i] = Matrix4.CreateScale(0.2f);
			}
			instanceTransforms[0] *= Matrix4.CreateScale((float)Math.Sin(time) * 0.5f + 0.7f);
			instanceTransforms[1] *= Matrix4.CreateTranslation(0, (float)Math.Sin(time) * 0.7f, 0);
			instanceTransforms[2] *= Matrix4.CreateRotationY(time);
			for (int i = 0; i < instanceTransforms.Length; ++i)
			{
				instanceTransforms[i] *= Matrix4.CreateTranslation((i - 1) * 0.65f, 0, 0);
			}
			var shader = shaderWatcher.Shader;
			//Matrix4 is stored row-major -> implies a transpose so in shader matrix is column major
			geometry.SetMatrixAttribute(shader.GetAttributeLocation("instanceTransform"), instanceTransforms, true);
		}

		private Matrix4[] instanceTransforms = new Matrix4[3];
		private ShaderFileDebugger shaderWatcher;
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			return VAOLoader.FromMesh(mesh, shader);
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}
	}
}
