using DMS.OpenGL;
using OpenTK.Platform;
using System.IO;
using System.Text;

namespace DMS.ShaderDebugging
{
	public class ShaderFileDebugger
	{
		public ShaderFileDebugger(string vertexFile, string fragmentFile, 
			byte[] vertexShader = null, byte [] fragmentShader = null,
			IGameWindow gameWindow = null)
		{
			GameWindow = gameWindow;
			if (File.Exists(vertexFile) && File.Exists(fragmentFile))
			{
				shaderWatcherVertex = new FileWatcher(vertexFile);
				shaderWatcherFragment = new FileWatcher(fragmentFile);
				CheckForShaderChange();
				while(!ReferenceEquals(null, LastException))
				{
					form.Hide();
					form.Show(LastException, GameWindow);
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
				form.Show(LastException, GameWindow);
			}
			catch (ShaderException e)
			{
				LastException = e;
				form.Show(e, GameWindow);
			}
			return false;
		}

		public IGameWindow GameWindow { get; private set; }

		public Shader Shader { get { return shader; } }
		public ShaderException LastException { get; private set; }

		private Shader shader;
		private readonly FileWatcher shaderWatcherVertex = null;
		private readonly FileWatcher shaderWatcherFragment = null;
		private readonly FormShaderExceptionFacade form = new FormShaderExceptionFacade();
	}
}
