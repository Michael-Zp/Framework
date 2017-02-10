using DMSOpenGL;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
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
			GL.Enable(EnableCap.CullFace);
			timeSource.Start();
		}

		public void Render()
		{
			var time = (float)timeSource.Elapsed.TotalSeconds;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			Matrix4 camera = CalculateCameraMatrixColVectorStyle(time);
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref camera);
			geometry.Draw(particelCount);
			shader.Deactivate();
		}

		private static Matrix4 CalculateCameraMatrix(float time)
		{
			var p = Matrix4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100.0f);
			var s = Matrix4.CreateScale(0.1f);
			var t = Matrix4.CreateTranslation(0, 0, -10.0f);
			var r = Matrix4.CreateRotationY(0.1f * time);
			var camera = Matrix4.Transpose(s * r * t * p);
			return camera;
		}

		private static Matrix4 CalculateCameraMatrixColVectorStyle(float time)
		{
			var p = Matrix4.Transpose(Matrix4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100.0f));
			var t = Matrix4.Transpose(Matrix4.CreateTranslation(0, 0, -10.0f));
			var r = Matrix4.Transpose(Matrix4.CreateRotationY(0.1f * time));
			var s = Matrix4.Transpose(Matrix4.CreateScale(0.1f));
			var camera = p * t * r * s;
			return camera;
		}

		private const int particelCount = 500;
		private Shader shader;
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
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
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 8.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			vao.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			Func<float> RndSpeed = () => (Rnd01() - 0.5f);
			var instanceSpeeds = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instanceSpeeds[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
			}
			vao.SetAttribute(shader.GetAttributeLocation("instanceSpeed"), instanceSpeeds, VertexAttribPointerType.Float, 3, true);
		}
	}
}
