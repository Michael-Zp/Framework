using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using Zenseless.Base;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual()
		{
			camera.FarClip = 500;
			camera.Distance = 30;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			UpdateMesh(shader);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			var time = gameTime.Seconds;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "time"), time);
			float[] cam = camera.CalcMatrix().ToArray();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), 1, false, cam);
			geometry.Draw(particelCount);
			shader.Deactivate();
		}

		public static readonly string ShaderName = nameof(shader);
		private CameraOrbit camera = new CameraOrbit();
		private const int particelCount = 500;

		private IShader shader;
		private GameTime gameTime = new GameTime();
		private VAO geometry;

		private void UpdateMesh(IShader shader)
		{
			var mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry = VAOLoader.FromMesh(mesh, shader);

			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 8.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			Func<float> RndSpeed = () => (Rnd01() - 0.5f);
			var instanceSpeeds = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instanceSpeeds[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
			}
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "instanceSpeed"), instanceSpeeds, VertexAttribPointerType.Float, 3, true);
		}
	}
}
