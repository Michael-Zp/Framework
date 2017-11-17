using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Example
{
	class MainVisual
	{
		public MainVisual()
		{
			GL.Enable(EnableCap.ProgramPointSize);
			GL.Enable(EnableCap.PointSprite);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
		}

		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			UpdateGeometry(shader);
		}

		public void Render(float time)
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shader.Activate();
			////ATTENTION: always give the time as a float if the uniform in the shader is a float
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "time"), time);
			geometry.Draw();
			shader.Deactivate();
		}

		private const int pointCount = 500;
		private IShader shader;
		private VAO geometry;

		private void UpdateGeometry(IShader shader)
		{
			geometry = new VAO(PrimitiveType.Points);
			//generate position array on CPU
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var positions = new Vector2[pointCount];
			for (int i = 0; i < pointCount; ++i)
			{
				positions[i] = new Vector2(RndCoord(), RndCoord());
			}
			//copy positions to GPU
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "in_position"), positions, VertexAttribPointerType.Float, 2);
			//generate velocity arrray on CPU
			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.1f;
			var velocities = new Vector2[pointCount];
			for (int i = 0; i < pointCount; ++i)
			{
				velocities[i] = new Vector2(RndSpeed(), RndSpeed());
			}
			//copy velocities to GPU
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "in_velocity"), velocities, VertexAttribPointerType.Float, 2);
		}
	}
}
