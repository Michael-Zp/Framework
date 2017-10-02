﻿using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System.Text;
using OpenTK;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
	public class VisualPlane
	{
		public VisualPlane()
		{
			var sVertex = Encoding.UTF8.GetString(Resourcen.plane_vert);
			var sFragment = Encoding.UTF8.GetString(Resourcen.plane_frag);
			shdPlane = ShaderLoader.FromStrings(sVertex, sFragment);
			var mesh = Meshes.CreatePlane(2, 2, 1, 1);
			plane = VAOLoader.FromMesh(mesh, shdPlane);
		}

		public void Draw(Matrix4 cam)
		{
			if (ReferenceEquals(shdPlane, null)) return;
			GL.Disable(EnableCap.CullFace);
			shdPlane.Activate();
			GL.UniformMatrix4(shdPlane.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);

			plane.Draw();
			shdPlane.Deactivate();
			GL.Enable(EnableCap.CullFace);
		}

		private VAO plane = new VAO();
		private IShader shdPlane;
	}
}