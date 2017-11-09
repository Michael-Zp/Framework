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
			CameraDistance = 10.0f;
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public float CameraDistance { get; set; }
		public float CameraAzimuth { get; set; }
		public float CameraElevation { get; set; }

		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			var mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry = VAOLoader.FromMesh(mesh, shader);

			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 35.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref camera);
			geometry.Draw(particelCount);
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
			//todo student: use CameraDistance, CameraAzimuth, CameraElevation
			var p = Matrix4.Transpose(Matrix4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100.0f));
			camera = p;
		}

		private const int particelCount = 500;

		private IShader shader;
		private Matrix4 camera = Matrix4.Identity;
		private VAO geometry;
	}
}
