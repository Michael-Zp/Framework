using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ShaderDebugging;
using System;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			shaderWatcher = new ShaderFileDebugger("../../SSBOExample/Resources/vertex.vert"
				, "../../SSBOExample/Resources/fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);

			InitParticles();
		}

		private void InitParticles()
		{
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;

			buffer = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Vector2[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				data[i] = new Vector2(RndCoord(), RndCoord());
			}
			buffer.Set(data, BufferUsageHint.StaticRead);
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
			}
			GL.PointSize(3.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			var shader = shaderWatcher.Shader;
			shader.Begin();
			buffer.ActivateBind(3);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);

			buffer.Deactive();
			shader.End();
		}

		private ShaderFileDebugger shaderWatcher;
		private BufferObject buffer;
		private const int particelCount = 2000;
	}
}
