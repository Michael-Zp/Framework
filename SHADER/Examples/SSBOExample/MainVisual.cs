using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ShaderDebugging;
using System;

namespace Example
{
	struct Particle
	{
		public Vector2 position;
		public Vector2 velocity;
	}

	public class MainVisual
	{
		public MainVisual()
		{
			shaderWatcher = new ShaderFileDebugger("../../SSBOExample/Resources/vertex.vert"
				, "../../SSBOExample/Resources/fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);

			InitParticles();
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
			}
			GL.PointSize(2.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			var shader = shaderWatcher.Shader;
			shader.Activate();
			var bindingIndex = shader.GetShaderStorageBufferBindingIndex("BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			bufferParticles.Deactive();
			shader.Deactivate();
		}

		private ShaderFileDebugger shaderWatcher;
		private BufferObject bufferParticles;
		private const int particelCount = 50000;

		private void InitParticles()
		{
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.01f;

			bufferParticles = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Particle[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				data[i].position = new Vector2(RndCoord(), RndCoord());
				data[i].velocity = new Vector2(RndSpeed(), RndSpeed());
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
