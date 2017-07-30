using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DMS.HLGL;

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

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry = VAOLoader.FromMesh(mesh, shader);
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
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions.ToArray(), VertexAttribPointerType.Float, 3, true);
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "instanceScale"), instanceScale.ToArray(), VertexAttribPointerType.Float, 1, true);

			var time = (float)timeSource.Elapsed.TotalSeconds;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "time"), time);
			Matrix4 cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			geometry.Draw(instancePositions.Count);
			shader.Deactivate();
		}

		private IShader shader;
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometry;
		private CameraOrbit camera = new CameraOrbit();
	}
}
