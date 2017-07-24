using DMS.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using System.Diagnostics;
using DMS.Geometry;
using DMS.HLGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderContext context)
		{
			camera.FarClip = 20;
			camera.Distance = 3;
			camera.FovY = 70;
			camera.Elevation = 15;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			timeSource.Start();
		}

		public static readonly string ShaderName = nameof(shader);

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void ShaderChanged(string name, Shader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			InitParticles();
		}

		public void Render()
		{
			if (ReferenceEquals(null, shader)) return;

			for (int i = 0; i < instanceCount; ++i)
			{
				instancePosition[i] += instanceVelocity[i];
				var abs = Vector3.One - Vector3.Abs(instancePosition[i]);
				if(abs.X < 0 || abs.Y < 0 || abs.Z < 0)
				{
					instanceVelocity[i] = -instanceVelocity[i];
				}
				instanceRotation[i] += 0.2f;
			}
			geometry.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePosition, VertexAttribPointerType.Float, 3, true);
			geometry.SetAttribute(shader.GetAttributeLocation("instanceRotation"), instanceRotation, VertexAttribPointerType.Float, 1, true);


			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			geometry.Draw(instanceCount);
			shader.Deactivate();
		}

		private Shader shader;
		private Stopwatch timeSource = new Stopwatch();
		private const int instanceCount = (int)30000;
		private Vector3[] instancePosition = new Vector3[instanceCount];
		private Vector3[] instanceVelocity = new Vector3[instanceCount];
		private float[] instanceRotation = new float[instanceCount];
		private Vector3[] instanceColor = new Vector3[instanceCount];
		private CameraOrbit camera = new CameraOrbit();
		private VAO geometry;

		private void InitParticles()
		{
			//geometry = VAOLoader.FromMesh(Meshes.CreateSphere(0.01f, 2), shader);
			geometry = VAOLoader.FromMesh(Obj2Mesh.FromObj(Resourcen.suzanne).Transform(Matrix4x4.CreateScale(0.01f)), shader);

			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.01f;
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePosition[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
				instanceVelocity[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
				instanceColor[i] = new Vector3(0.5f) + instancePosition[i] * 0.5f;
			}
			geometry.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePosition, VertexAttribPointerType.Float, 3, true);
			geometry.SetAttribute(shader.GetAttributeLocation("instanceColor"), instanceColor, VertexAttribPointerType.Float, 3, true);
		}
	}
}
