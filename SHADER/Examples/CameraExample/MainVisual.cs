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
	public class MainVisual : IWindow
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual()
		{
			camera.FarClip = 500;
			camera.Distance = 30;

			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);
			var shader = shaderWatcher.Shader;

			geometry = CreateMesh(shader);
			CreatePerInstanceAttributes(geometry, shader);

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
				CreatePerInstanceAttributes(geometry, shader);
			}

			var time = (float)timeSource.Elapsed.TotalSeconds;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			float[] cam = camera.CalcMatrix().ToArray();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), 1, false, cam);
			geometry.Draw(particelCount);
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
		}

		private CameraOrbit camera = new CameraOrbit();
		private const int particelCount = 500;
		private ShaderFileDebugger shaderWatcher;
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			var vao = VAOLoader.FromMesh(mesh, shader);
			return vao;
		}

		private static void CreatePerInstanceAttributes(VAO vao, Shader shader)
		{
			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 8.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			vao.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			Func<float> RndSpeed = () => (Rnd01() - 0.5f);
			var instanceSpeeds = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instanceSpeeds[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
			}
			vao.SetAttribute(shader.GetAttributeLocation("instanceSpeed"), instanceSpeeds, VertexAttribPointerType.Float, 3, true);
		}
	}
}
