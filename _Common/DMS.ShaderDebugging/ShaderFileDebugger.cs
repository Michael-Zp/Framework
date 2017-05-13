using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;

namespace DMS.ShaderDebugging
{
	public class ShaderFileDebugger
	{
		public delegate void ShaderLoadedHandler();
		public event ShaderLoadedHandler ShaderLoaded;

		public ShaderFileDebugger(string vertexFile, string fragmentFile,
			byte[] vertexShader = null, byte[] fragmentShader = null)
		{
			if (File.Exists(vertexFile) && File.Exists(fragmentFile))
			{
				shaderWatcherVertex = new FileWatcher(vertexFile);
				shaderWatcherVertex.Changed += (s, e) => form.Close();
				shaderWatcherFragment = new FileWatcher(fragmentFile);
				shaderWatcherFragment.Changed += (s, e) => form.Close();
			}
			else
			{
				var sVertex = Encoding.UTF8.GetString(vertexShader);
				var sFragment = Encoding.UTF8.GetString(fragmentShader);
				shader = ShaderLoader.FromStrings(sVertex, sFragment);
				ShaderLoaded?.Invoke();
			}
		}

		public bool CheckForShaderChange()
		{
			//test if we even have file -> no files nothing to be done
			if (ReferenceEquals(null, shaderWatcherVertex) || ReferenceEquals(null, shaderWatcherFragment)) return false;
			//test if any file is dirty
			if (!shaderWatcherVertex.Dirty && !shaderWatcherFragment.Dirty) return false;
			try
			{
				shader = ShaderLoader.FromFiles(shaderWatcherVertex.FullPath, shaderWatcherFragment.FullPath);
				shaderWatcherVertex.Dirty = false;
				shaderWatcherFragment.Dirty = false;
				ShaderLoaded?.Invoke();
				return true;
			}
			catch (IOException e)
			{
				var exception = new ShaderException(e.Message, string.Empty);
				ShowDebugDialog(exception);
			}
			catch (ShaderException e)
			{
				ShowDebugDialog(e);
			}
			return false;
		}

		private void ShowDebugDialog(ShaderException exception)
		{
			var newShaderCode = form.ShowModal(exception);
			var compileException = exception as ShaderCompileException;
			if (ReferenceEquals(null, compileException)) return;
			if (newShaderCode != compileException.ShaderCode)
			{
				//save changed code to shaderfile
				switch(compileException.ShaderType)
				{
					case ShaderType.VertexShader: File.WriteAllText(shaderWatcherVertex.FullPath, newShaderCode); break;
					case ShaderType.FragmentShader: File.WriteAllText(shaderWatcherFragment.FullPath, newShaderCode); break;
					default: throw new ArgumentOutOfRangeException("ShowDebugDialog called with invalid shader type", compileException);
				}
			}
		}

		public Shader Shader { get { return shader; } }

		private Shader shader;
		private readonly FileWatcher shaderWatcherVertex = null;
		private readonly FileWatcher shaderWatcherFragment = null;
		private readonly FormShaderExceptionFacade form = new FormShaderExceptionFacade();
	}
}
