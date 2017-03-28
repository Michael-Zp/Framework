using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using DMS.ShaderDebugging;
using System.IO;
using DMS.System;

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
			//GL.Enable(EnableCap.LineSmooth);
			//GL.Enable(EnableCap.Blend);
			////setup blending equation Color = Color_s · alpha + Color_d · (1 - alpha)
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.BlendEquation(BlendEquationMode.FuncAdd);
			timeSource.Start();
		}

		public void Render()
		{
			var time = (float)timeSource.Elapsed.TotalSeconds;
			var shader = shaderWatcher.Shader;
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				geometry = CreateMesh(shader);
			}
			//per instance attributes
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
			//Matrix4 is stored row-major
			geometry.SetMatrixAttribute(shader.GetAttributeLocation("instanceTransform"), instanceTransforms, true);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			Matrix4 camera = Matrix4.CreateScale(1, 1, -1);
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref camera);
			geometry.Draw(instanceTransforms.Length);
			shader.Deactivate();
		}

		private Matrix4[] instanceTransforms = new Matrix4[3];
		private ShaderFileDebugger shaderWatcher;
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			var vao = new VAO();
			vao.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}

		public void Update(float updatePeriod)
		{
		}

		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			//app.GameWindow.WindowState = WindowState.Fullscreen;
			app.Run(new MyWindow());
		}
	}
}
