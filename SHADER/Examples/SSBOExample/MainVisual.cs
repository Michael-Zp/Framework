using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Numerics;
using System.Diagnostics;
using DMS.Geometry;

namespace Example
{
	//[StructLayout(LayoutKind.Sequential, Pack = 1)] // does not help with required shader alignment, affects only cpu part
	struct Particle //use 16 byte alignment or you have to query all variable offsets
	{
		public Vector2 position;
		public Vector2 velocity; //position + velocity are aligned to 16byte
		public Vector3 color;
		public float size; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
	}

	public class MainVisual
	{
		public MainVisual()
		{
			InitParticles();
			GL.Enable(EnableCap.ProgramPointSize);
			GL.Enable(EnableCap.PointSprite);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			timeSource.Start();
		}

		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, Shader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
		}

		public void Render()
		{
			if (ReferenceEquals(null, shader)) return;
			var time = (float)timeSource.Elapsed.TotalSeconds;
			var deltaTime = time - lastRenderTime;
			lastRenderTime = time;
			Console.Write(Math.Round(timeQuery.ResultLong * 1e-6, 2));
			Console.WriteLine("msec");
			timeQuery.Activate(QueryTarget.TimeElapsed);
			GL.PointSize(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shader.Activate();
			GL.Uniform1(shader.GetUniformLocation("deltaTime"), deltaTime);
			GL.Uniform1(shader.GetUniformLocation("particelCount"), particelCount);
			var bindingIndex = shader.GetShaderStorageBufferBindingIndex("BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			bufferParticles.Deactivate();
			shader.Deactivate();
			timeQuery.Deactivate();
		}

		private Shader shader;
		private BufferObject bufferParticles;
		private QueryObject timeQuery = new QueryObject();
		private Stopwatch timeSource = new Stopwatch();
		private float lastRenderTime = 0f;
		private const int particelCount = (int)1e4;

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
				data[i].position = new Vector2(RndCoord(), RndCoord());
				data[i].velocity = new Vector2(RndSpeed(), RndSpeed());
				var polar = MathHelper.ToPolar(data[i].position);
				var color = ColorSystems.Hsb2rgb(polar.X / MathHelper.TWO_PI + 0.5f, polar.Y, 1);
				var byteColor = ColorSystems.ToSystemColor(color);
				data[i].color = color;
				data[i].size = (Rnd01() + 1) * 10;
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
