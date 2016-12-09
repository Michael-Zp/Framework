using Framework;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			camera.FarClip = 500;
			camera.Distance = 30;
			var sVertex = Encoding.UTF8.GetString(Resourcen.vertex);
			var sFragment = Encoding.UTF8.GetString(Resourcen.fragment);
			shader = ShaderLoader.FromStrings(sVertex, sFragment);
			geometry = CreateMesh(shader);

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			timeSource.Start();
		}

		public CameraOrbit Camera { get { return camera; } }

		public void Render(IEnumerable<IBody> bodies)
		{
			var instancePositions = new List<Vector3>();
			var instanceScale = new List<float>();
			foreach (var body in bodies)
			{
				instancePositions.Add(body.Location);
				instanceScale.Add((float)Math.Pow(body.Mass, 0.33f));
			}
			geometry.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions.ToArray(), VertexAttribPointerType.Float, 3, true);
			geometry.SetAttribute(shader.GetAttributeLocation("instanceScale"), instanceScale.ToArray(), VertexAttribPointerType.Float, 1, true);

			var time = (float)timeSource.Elapsed.TotalSeconds;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			Matrix4 cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			geometry.Draw(instancePositions.Count);
			shader.End();
		}

		private Shader shader;
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometry;
		private CameraOrbit camera = new CameraOrbit();

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			var vao = new VAO();
			vao.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}
	}
}
