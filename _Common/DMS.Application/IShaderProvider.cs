using DMS.HLGL;
using DMS.OpenGL;

namespace DMS.Application
{
	public interface IShaderProvider
	{
		//public delegate void ShaderChangedHandler(string name, Shader shader);
		//public event ShaderChangedHandler ShaderChanged;

		IShader GetShader(string name);
		//Texture GetTexture(string name);
	}
}