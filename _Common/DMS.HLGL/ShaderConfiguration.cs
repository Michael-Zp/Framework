using DMS.OpenGL;

namespace DMS.HLGL
{
	public class ShaderConfiguration
	{
		public ShaderConfiguration(Shader shader) //todo: give only shader code
		{
			Shader = shader;
		}

		//public string Name { get; private set; }
		//todo parameters: images, buffers

		public Shader Shader { get; private set; }
	}
}