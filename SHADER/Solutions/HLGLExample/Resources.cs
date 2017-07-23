using DMS.Application;
using DMS.HLGL;
using DMS.OpenGL;
using System.Text;

namespace Example
{
	public static class Resources
	{
		public const string ShaderDefault = "shader";
		public const string ShaderCopy = "copy";
		public const string TextureDiffuse = "diffuse";

		public static void LoadResources(IResourceProvider resourceProvider)
		{
			//var assembly = Assembly.GetExecutingAssembly();
			//var names = assembly.GetManifestResourceNames();
			//var imageStream = assembly.GetManifestResourceStream("Example.Resources.ps8k_height.png");
			
			//var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			//resourceManager.Add<Shader>("shader", new ResourceVertFragShaderFile(dir + "vertex.vert", dir + "fragment.frag"));
			resourceProvider.Add(ShaderDefault, new ResourceVertFragShaderString(Encoding.UTF8.GetString(Resourcen.vertex), Encoding.UTF8.GetString(Resourcen.fragment)));
			resourceProvider.Add(ShaderCopy, new ResourceVertFragShaderString(TextureToFrameBuffer.VertexShaderScreenQuad, TextureToFrameBuffer.FragmentShaderCopy));
			//resourceManager.AddShader("shader", dir + "vertex.vert", dir + "fragment.frag"
			//	, Resourcen.vertex, Resourcen.fragment);
			resourceProvider.Add(TextureDiffuse, new ResourceTextureBitmap(Resourcen.chalet));
		}
	}
}
