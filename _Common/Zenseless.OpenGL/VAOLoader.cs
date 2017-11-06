using Zenseless.Geometry;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Provides static methods for VertexArrayObject data loading
	/// </summary>
	public static class VAOLoader
	{
		/// <summary>
		/// Creates a VertexArrayObject from a mesh expecting the MeshAttribute names as shader variable names for the attributes 
		/// </summary>
		/// <param name="mesh">From which to load positions, indices, normals, texture coordinates</param>
		/// <param name="shader">Used for the attribute location bindings</param>
		/// <returns>A vertex array object</returns>
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
			vao.SetIndex(mesh.IDs.ToArray());
			vao.PrimitiveType = PrimitiveType.Triangles;
			return vao;
		}
	}
}
