using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShaderForm
{
	public class Shaders : IDisposable, IShaders
	{
		public event EventHandler<string> OnChange;

		public delegate IShaderFile ShaderFileCreator();
		public Shaders(VisualContext visual, ShaderFileCreator shaderCreator)
		{
			this.visual = visual;
			this.shaderCreator = shaderCreator;
		}

		public void AddUpdateShader(string shaderFileName)
		{
			if (!File.Exists(shaderFileName))
			{
				CallOnChange("Could not find shader '" + shaderFileName + "'");
				return;
			}
			if (shaders.ContainsKey(shaderFileName)) return;
			var shader = shaderCreator();
			shader.OnChange += (sender, message) => CallOnChange(message);
			shaders[shaderFileName] = shader;
			shader.Load(shaderFileName);
		}

		public void Clear()
		{
			try
			{
				foreach (var shader in shaders.Values) shader.Dispose();
				shaders.Clear();
			}
			finally
			{
				CallOnChange(string.Empty);
			}
		}

		public void Dispose()
		{
			visual = null;
			shaderCreator = null;
		}

		public void RemoveShader(string shaderFileName)
		{
			IShaderFile shader;
			if (shaders.TryGetValue(shaderFileName, out shader))
			{
				shader.Dispose();
			}
			shaders.Remove(shaderFileName);

			CallOnChange(string.Empty);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return shaders.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return shaders.Keys.GetEnumerator();
		}

		protected void CallOnChange(string message)
		{
			OnChange?.Invoke(this, message);
		}

		private Dictionary<string, IShaderFile> shaders = new Dictionary<string, IShaderFile>();

		private VisualContext visual;
		private ShaderFileCreator shaderCreator;
	}
}
