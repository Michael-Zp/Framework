using System;
using DMS.OpenGL;
using DMS.ShaderDebugging;
using System.Collections.Generic;

namespace DMS.Application
{
	public class ResourceManager : IShaderProvider
	{
		//public delegate void ResourceChangedHandler(ResourceManager manager, string name);
		//public event ResourceChangedHandler ResourceChanged;
		public delegate void ShaderChangedHandler(string name, Shader shader);
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

		public void AddShader(string name, string vertexFile, string fragmentFile,
			byte[] vertexShaderResource = null, byte[] fragmentShaderResource = null)
		{
			var sfd = new ShaderFileDebugger(vertexFile, fragmentFile, vertexShaderResource, fragmentShaderResource);
			shaderWatcher.Add(name, sfd);
		}

		public void CheckForShaderChange()
		{
			foreach(var item in shaderWatcher)
			{
				if(item.Value.CheckForShaderChange())
				{
					ShaderChanged?.Invoke(item.Key, item.Value.Shader);
				}
			}
		}

		public Shader GetShader(string name)
		{
			ShaderFileDebugger shaderFD;
			if(shaderWatcher.TryGetValue(name, out shaderFD))
			{
				return shaderFD.Shader;
			}
			return null;
		}

		private Dictionary<string, ShaderFileDebugger> shaderWatcher = new Dictionary<string, ShaderFileDebugger>();
	}
}