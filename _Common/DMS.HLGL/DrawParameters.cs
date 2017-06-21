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
		public VAO Vao { get; set; }
		//public ShaderConfiguration ShaderConfiguration { get; set; }
		public Shader Shader { get; set; }
		public List<NamedTexture> Textures { get; private set; } = new List<NamedTexture>();
		public bool ZBufferTest { get; set; }
	}
}