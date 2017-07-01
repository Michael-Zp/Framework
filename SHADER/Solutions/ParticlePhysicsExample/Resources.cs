using DMS.Application;
using DMS.OpenGL;
using System.Text;

namespace Example
{
	public static class Resources
	{
		public const string ShaderPaintObstacles = nameof(ShaderPaintObstacles);
		public const string ShaderCopy = nameof(ShaderCopy);

		public static void LoadResources(ResourceManager resourceManager)
		{
			resourceManager.Add<Shader>(ShaderPaintObstacles, new ResourceVertFragShaderString(TextureToFrameBuffer.VertexShaderScreenQuad, Encoding.UTF8.GetString(Resourcen.paintObstacles)));
			resourceManager.Add<Shader>(ShaderCopy, new ResourceVertFragShaderString(TextureToFrameBuffer.VertexShaderScreenQuad, TextureToFrameBuffer.FragmentShaderCopy));
		}
	}
}
