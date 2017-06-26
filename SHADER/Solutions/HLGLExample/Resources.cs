using DMS.Application;
using DMS.OpenGL;
using System.Text;

namespace Example
{
	public static class Resources
	{
		public const string Shader = "shader";
		public const string ShaderCopy = "copy";
		public const string TextureDiffuse = "diffuse";

		public static void LoadResources(ResourceManager resourceManager)
		{
			//var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			//resourceManager.Add<Shader>("shader", new ResourceVertFragShaderFile(dir + "vertex.vert", dir + "fragment.frag"));
			resourceManager.Add<Shader>(Shader, new ResourceVertFragShaderString(Encoding.UTF8.GetString(Resourcen.vertex), Encoding.UTF8.GetString(Resourcen.fragment)));
			resourceManager.Add<Shader>(ShaderCopy, new ResourceVertFragShaderString(TextureToFrameBuffer.VertexShaderScreenQuad, TextureToFrameBuffer.FragmentShaderCopy));
			//resourceManager.AddShader("shader", dir + "vertex.vert", dir + "fragment.frag"
			//	, Resourcen.vertex, Resourcen.fragment);
			resourceManager.Add<Texture>(TextureDiffuse, new ResourceTextureBitmap(Resourcen.capsule0));
		}
	}
}
