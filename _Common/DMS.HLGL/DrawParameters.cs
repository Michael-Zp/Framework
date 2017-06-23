using DMS.Application;
using DMS.Geometry;
using DMS.OpenGL;
using System.Collections.Generic;

namespace DMS.HLGL
{
	public class NamedTexture
	{
		public NamedTexture(string name, Texture texture)
		{
			Texture = texture;
			Name = name;
		}

		public string Name { get; private set; }
		public Texture Texture { get; private set; }
	}

	public class DrawParameters
	{
		public bool BackfaceCulling { get; set; }
		//public Geometry Geometry { get; set; }
		public VAO Vao { get; private set; }
		public void UpdateMesh(Mesh mesh)
		{
			if (this.mesh == mesh) return;
			this.mesh = mesh;
			if (ReferenceEquals(null, mesh)) return;
			if(!ReferenceEquals(null, Shader)) UpdateVao();
		}

		//public ShaderConfiguration ShaderConfiguration { get; set; }
		public Shader Shader { get; private set; }
		public void UpdateShader(string shaderName)
		{
			if (this.shaderName == shaderName) return;
			this.shaderName = shaderName;
			if (ReferenceEquals(null, shaderName)) return;
			Shader = ResourceManager.Instance.Get<Shader>(shaderName).Value;
			if(!ReferenceEquals(mesh, null)) UpdateVao();
		}

		public List<NamedTexture> Textures { get; private set; } = new List<NamedTexture>();
		public bool ZBufferTest { get; set; }

		private string shaderName;
		private Mesh mesh;

		private void UpdateVao()
		{
			Vao = VAOLoader.FromMesh(mesh, Shader);
		}
	}
}