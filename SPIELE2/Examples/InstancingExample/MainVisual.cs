using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Text;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			var sVertex = Encoding.UTF8.GetString(Resourcen.vertex);
			var sFragment = Encoding.UTF8.GetString(Resourcen.fragment);
			shader = ShaderLoader.FromStrings(sVertex, sFragment);

			geometry = CreateMesh(shader);

			CreatePerInstanceAttributes(geometry, shader);

			GL.Enable(EnableCap.DepthTest);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			geometry.Draw(particelCount);
			shader.End();
		}

		private const int particelCount = 10000;
		private Shader shader;
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Meshes.CreateSphere(0.03f, 2);
			var vao = new VAO();
			vao.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}

		private static void CreatePerInstanceAttributes(VAO vao, Shader shader)
		{
			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			vao.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			//todo: students: add per instance attribute speed here
			//var locInstSpeed = shader.GetAttributeLocation("instanceSpeed");
		}
	}
}
