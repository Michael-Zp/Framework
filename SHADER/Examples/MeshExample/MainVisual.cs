﻿using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Text;
using System;

namespace Example
{
	public class MainVisual : IWindow
	{
		public MainVisual()
		{
			var sVertex = Encoding.UTF8.GetString(Resourcen.vertex);
			var sFragment = Encoding.UTF8.GetString(Resourcen.fragment);
			shader = ShaderLoader.FromStrings(sVertex, sFragment);

			//load geometry
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			geometry.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			//geometry.SetAttribute(shader.GetAttributeLocation("uv"), mesh.uvs.ToArray(), VertexAttribPointerType.Float, 2);
			geometry.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //draw wireframe

			//GL.Enable(EnableCap.DepthTest);
			//texDiffuse = TextureLoader.FromFile(pathMedia + "capsule0.jpg");
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			//texDiffuse.Activate();
			geometry.Draw();
			//texDiffuse.Deactivate();
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
		}

		private Shader shader;
		private VAO geometry = new VAO();
		//private Texture texDiffuse;
	}
}
