using Zenseless.Geometry;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	public static class VAOLoader
	{
		/// <summary>
		/// Creates a VertexArrayObject from a mesh expecting the MeshAttribute names as shader variable names for the attributes 
		/// </summary>
		/// <param name="mesh">to load to the VertexArrayObject</param>
		/// <param name="shader">for the attribute location bindings</param>
		/// <returns></returns>
		public static VAO FromMesh(Mesh mesh, IShader shader)
		{
			var vao = new VAO();
			if (mesh.Position.List.Count > 0)
			{
				var loc = shader.GetResourceLocation(ShaderResourceType.Attribute, mesh.Position.Name);
				vao.SetAttribute(loc, mesh.Position.List.ToArray(), VertexAttribPointerType.Float, 3);
			}
			if (mesh.Normal.List.Count > 0)
			{
				var loc = shader.GetResourceLocation(ShaderResourceType.Attribute, mesh.Normal.Name);
				vao.SetAttribute(loc, mesh.Normal.List.ToArray(), VertexAttribPointerType.Float, 3);
			}
			if (mesh.Uv.List.Count > 0)
			{
				var loc = shader.GetResourceLocation(ShaderResourceType.Attribute, mesh.Uv.Name);
				vao.SetAttribute(loc, mesh.Uv.List.ToArray(), VertexAttribPointerType.Float, 2);
			}
			vao.SetID(mesh.IDs.ToArray());
			vao.PrimitiveType = PrimitiveType.Triangles;
			return vao;
		}
	}
}
