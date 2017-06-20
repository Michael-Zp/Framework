using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			camera.FarClip = 500;
			camera.Distance = 30;
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			timeSource.Start();
		}

		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, Shader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometryBody = VAOLoader.FromMesh(mesh, shader);

			var plane = Meshes.CreateQuad(100, 100, 10, 10);
			var xForm = new Transformation();
			xForm.TranslateLocal(0, -20, 0);
			geometryPlane = VAOLoader.FromMesh(plane.Transform(xForm), shader);
			//for AMD graphics cards
			geometryPlane.SetAttribute(shader.GetAttributeLocation("instancePosition"), new Vector3[] { Vector3.Zero }, VertexAttribPointerType.Float, 3, true);
			geometryPlane.SetAttribute(shader.GetAttributeLocation("instanceScale"), new float[] { 1 }, VertexAttribPointerType.Float, 1, true);

		}

		public CameraOrbit Camera { get { return camera; } }

		public void Render(IEnumerable<IBody> bodies)
		{
			if (ReferenceEquals(null, shader)) return;
			var instancePositions = new List<Vector3>();
			var instanceScale = new List<float>();
			foreach (var body in bodies)
			{
				instancePositions.Add(body.Location);
				instanceScale.Add((float)Math.Pow(body.Mass, 0.33f));
			}
			geometryBody.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions.ToArray(), VertexAttribPointerType.Float, 3, true);
			geometryBody.SetAttribute(shader.GetAttributeLocation("instanceScale"), instanceScale.ToArray(), VertexAttribPointerType.Float, 1, true);

			var time = (float)timeSource.Elapsed.TotalSeconds;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			Matrix4 cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			geometryBody.Draw(instancePositions.Count);
			//geometryPlane.Draw(); //todo student: uncomment for gravity
			shader.Deactivate();
		}

		private Shader shader;
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometryBody, geometryPlane;
		private CameraOrbit camera = new CameraOrbit();
	}
}
