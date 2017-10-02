using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			GL.Enable(EnableCap.DepthTest);
		}

		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			UpdateGeometry(shader);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			geometry.Draw(instanceCount);
			shader.Deactivate();
		}

		private const int instanceCount = 10000;
		private IShader shader;

		private VAO geometry;

		private void UpdateGeometry(IShader shader)
		{
			Mesh mesh = Meshes.CreateSphere(0.03f, 2);
			geometry = VAOLoader.FromMesh(mesh, shader);

			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var instancePositions = new Vector3[instanceCount];
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			//todo students: add per instance attribute speed here
		}
	}
}
