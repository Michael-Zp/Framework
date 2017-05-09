using System;

namespace DMS.OpenGL
{
	public class ResourceManager
	{
		public event Action<ResourceManager, string> ResourceChanged;

		public void AddWatchedFileResource(string v1, string v2)
		{
		}

		public object Get(string resourceName)
		{
			return null;
		}

		public string GetString(string resourceName)
		{
			return null;
		}
	}
}