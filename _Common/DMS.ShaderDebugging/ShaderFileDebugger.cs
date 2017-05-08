using DMS.OpenGL;
using System;
using System.IO;
using System.Text;

namespace DMS.ShaderDebugging
{
	public class ShaderFileDebugger
	{
		public ShaderFileDebugger(string vertexFile, string fragmentFile, 
			byte[] vertexShader = null, byte [] fragmentShader = null)
		{
			if (File.Exists(vertexFile) && File.Exists(fragmentFile))
			{
				shaderWatcherVertex = new FileWatcher(vertexFile);
				shaderWatcherFragment = new FileWatcher(fragmentFile);
				CheckForShaderChange();
				while (!ReferenceEquals(null, LastException))
				{
					PrintShaderException(LastException);
					//form.Hide();
					//form.Show(LastException);
					//FormShaderExceptionFacade.ShowModal(LastException);
					CheckForShaderChange();
				}
			}
			else
			{
				var sVertex = Encoding.UTF8.GetString(vertexShader);
				var sFragment = Encoding.UTF8.GetString(fragmentShader);
				shader = ShaderLoader.FromStrings(sVertex, sFragment);
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
				form.Clear();
				form.Hide();
				return true;
			}
			catch (IOException e)
			{
				LastException = new ShaderException("ERROR", e.Message, string.Empty, string.Empty);
				PrintShaderException(LastException);
				//form.Show(LastException);
			}
			catch (ShaderException e)
			{
				LastException = e;
				PrintShaderException(e);
				//form.Show(e);
			}
			return false;
		}

		public Shader Shader { get { return shader; } }
		public ShaderException LastException { get; private set; }

		private Shader shader;
		private readonly FileWatcher shaderWatcherVertex = null;
		private readonly FileWatcher shaderWatcherFragment = null;
		private readonly FormShaderExceptionFacade form = new FormShaderExceptionFacade();
		private static void PrintShaderException(ShaderException e)
		{
			Console.Write(e.Type);
			Console.Write(": ");
			Console.WriteLine(e.Message);
			Console.WriteLine(e.Log);
		}
	}
}
