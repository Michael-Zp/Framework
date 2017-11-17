using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using System.Runtime.InteropServices;

namespace Example
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)] // does not help with required shader alignment, affects only cpu part
	struct Particle //use 16 byte alignment or you have to query all variable offsets
	{
		public Vector3 position;
		public float size; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
		public Vector3 velocity;
		public uint color; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
	}

	public class MainVisual
	{
		public MainVisual()
		{
			camera.FarClip = 20;
			camera.Distance = 2;
			camera.FovY = 70;
			camera.Elevation = 15;

			InitParticles();
			GL.Enable(EnableCap.ProgramPointSize);
			GL.Enable(EnableCap.PointSprite);
			GL.Enable(EnableCap.DepthTest);
		}

		public static readonly string ShaderName = nameof(shader);

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
		}

		public double Render(float deltaTime)
		{
			if (ReferenceEquals(null, shader)) return 0;
			var timerQueryResult = timeQuery.ResultLong * 1e-6;
			timeQuery.Activate(QueryTarget.TimeElapsed);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "deltaTime"), deltaTime);
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "particelCount"), particelCount);
			var bindingIndex = shader.GetResourceLocation(ShaderResourceType.RWBuffer, "BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			bufferParticles.Deactivate();
			shader.Deactivate();
			timeQuery.Deactivate();
			return timerQueryResult;
		}

		private IShader shader;
		private BufferObject bufferParticles;
		private QueryObject timeQuery = new QueryObject();
		private const int particelCount = (int)1e5;
		private CameraOrbit camera = new CameraOrbit();

		private void InitParticles()
		{
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.1f;

			bufferParticles = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Particle[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				var pos = new Vector3(RndCoord(), RndCoord(), RndCoord());
				data[i].position = pos;
				data[i].velocity = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
				var color = new Vector4(new Vector3(0.5f) + pos * 0.5f, 1);
				var packedColor = MathHelper.PackUnorm4x8(color);
				data[i].color = packedColor;
				data[i].size = (Rnd01() + 1) * 10;
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
