using System;
using Zenseless.OpenGL;
using Zenseless.ShaderDebugging;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Zenseless.HLGL;

namespace Zenseless.Application
{
	[Export(typeof(IResourceProvider))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class ResourceManager : IShaderProvider, IResourceProvider
	{
		public ResourceManager()
		{
			Instance = this;
		}

		public delegate void ShaderChangedHandler(string name, IShader shader);
		public event ShaderChangedHandler ShaderChanged;

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

		public IShader GetShader(string name)
		{
			if (shaderWatcher.TryGetValue(name, out ShaderFileDebugger shaderFD))
			{
				return shaderFD.Shader;
			}
			return null;
		}

		public void Add<RESOURCE_TYPE>(string name, IResource<RESOURCE_TYPE> resource) where RESOURCE_TYPE : IDisposable
		{
			resources.Add(name, resource); //throws exception if key exists
		}

		public IResource<RESOURCE_TYPE> Get<RESOURCE_TYPE>(string name) where RESOURCE_TYPE : IDisposable
		{
			if (resources.TryGetValue(name, out object resource))
			{
				return resource as IResource<RESOURCE_TYPE>;
			}
			return null;
		}

		private Dictionary<string, ShaderFileDebugger> shaderWatcher = new Dictionary<string, ShaderFileDebugger>();
		private Dictionary<string, object> resources = new Dictionary<string, object>();

		public static IResourceProvider Instance { get; private set; }
	}
}