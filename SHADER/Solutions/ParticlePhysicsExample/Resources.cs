using DMS.Application;
using DMS.HLGL;
using DMS.OpenGL;
using System.Text;

namespace Example
{
	public static class Resources
	{
		public const string ShaderPaintObstacles = nameof(ShaderPaintObstacles);
		public const string ShaderParticles = nameof(ShaderParticles);

		public static void LoadResources(ResourceManager resourceManager)
		{
			resourceManager.Add<IShader>(ShaderPaintObstacles, new ResourceVertFragShaderString(TextureToFrameBuffer.VertexShaderScreenQuad, Encoding.UTF8.GetString(Resourcen.paintObstacles)));
			resourceManager.Add<IShader>(ShaderParticles, new ResourceVertFragShaderString(Encoding.UTF8.GetString(Resourcen.particles), Encoding.UTF8.GetString(Resourcen.particlesFrag)));
		}
	}
}
