using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using DMS.ShaderDebugging;
using System;
using System.Numerics;
using System.IO;
using DMS.Base;
using DMS.Application;
using System.Diagnostics;

namespace Example
{
	struct Particle
	{
		public Vector2 position;
		public Vector2 velocity;
		//public Vector4 color; //make it vec4  not vec3 because of alignment in std430
	}

	public class MainVisual : IWindow
	{
		public MainVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);
			InitParticles();

			GL.Enable(EnableCap.ProgramPointSize);
			GL.Enable(EnableCap.PointSprite);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			timeSource.Start();
		}

		public void Render()
		{
			var time = (float)timeSource.Elapsed.TotalSeconds;
			var deltaTime = time - lastRenderTime;
			lastRenderTime = time;
			Console.WriteLine(timeQuery.ResultLong * 1e-9);
			if (shaderWatcher.CheckForShaderChange())
			{
				//should update geometry when shader changes -> attribute bindings may change
			}
			timeQuery.Activate(QueryTarget.TimeElapsed);
			GL.PointSize(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			var shader = shaderWatcher.Shader;
			if (ReferenceEquals(null, shader)) return;
			shader.Activate();
			GL.Uniform1(shader.GetUniformLocation("deltaTime"), deltaTime);
			GL.Uniform1(shader.GetUniformLocation("particelCount"), particelCount);
			var bindingIndex = shader.GetShaderStorageBufferBindingIndex("BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			bufferParticles.Deactive();
			shader.Deactivate();
			timeQuery.Deactivate();
		}

		public void Update(float updatePeriod)
		{
		}

		private ShaderFileDebugger shaderWatcher;
		private BufferObject bufferParticles;
		private QueryObject timeQuery = new QueryObject();
		private Stopwatch timeSource = new Stopwatch();
		private float lastRenderTime = 0f;
		private const int particelCount = (int)1e5;

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
				//var polar = MathHelper.ToPolar(data[i].position);
				//data[i].color = new Vector4(ColorSystems.Hsb2rgb(polar.X / MathHelper.TWO_PI + 0.5f, polar.Y, 1), 1);
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
