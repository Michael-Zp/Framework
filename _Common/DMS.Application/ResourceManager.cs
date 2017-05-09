using DMS.OpenGL;
using DMS.ShaderDebugging;

namespace DMS.Application
{
	public class ResourceManager
	{
		//public delegate void ResourceChangedHandler(ResourceManager manager, string name);
		//public event ResourceChangedHandler ResourceChanged;
		public delegate void ShaderChangedHandler(Shader shader);
		public event ShaderChangedHandler ShaderChanged;

		//public void AddWatchedFileResource(string v1, string v2)
		//{
		//}

		//public object Get(string resourceName)
		//{
		//	return null;
		//}

		//public string GetString(string resourceName)
		//{
		//	return null;
		//}

		public void AddShader(string vertexFile, string fragmentFile,
			byte[] vertexShader = null, byte[] fragmentShader = null)
		{
			shaderWatcher = new ShaderFileDebugger(vertexFile, fragmentFile, vertexShader, fragmentShader);
		}

		public void CheckForShaderChange()
		{
			if (ReferenceEquals(null, shaderWatcher)) return;
			if(shaderWatcher.CheckForShaderChange())
			{
				ShaderChanged?.Invoke(shaderWatcher.Shader);
			}
		}

		private ShaderFileDebugger shaderWatcher;
	}
}