using OpenTK.Graphics.OpenGL4;
using ShaderForm.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Zenseless.Base;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace ShaderForm.Visual
{
	public class VisualContext : Disposable, ISetUniform
	{
		public VisualContext()
		{
			GL.Disable(EnableCap.DepthTest);
			GL.ClearColor(1, 0, 0, 0);

			surface = new RenderSurfacePingPong();

			copyToScreen = new TextureToFrameBuffer();
			shaderDefault = ShaderLoader.FromStrings(DefaultShader.VertexShaderScreenQuad, DefaultShader.FragmentShaderChecker);
		}

		public void SetUniform(string uniformName, float value)
		{
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));
			GL.Uniform1(shaderCurrent.GetResourceLocation(ShaderResourceType.Uniform, uniformName), value);
		}

		public void SetUniform(string uniformName, float valueX, float valueY)
		{
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));
			GL.Uniform2(shaderCurrent.GetResourceLocation(ShaderResourceType.Uniform, uniformName), valueX, valueY);
		}

		public void SetUniform(string uniformName, float valueX, float valueY, float valueZ)
		{
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));
			GL.Uniform3(shaderCurrent.GetResourceLocation(ShaderResourceType.Uniform, uniformName), valueX, valueY, valueZ);
		}

		public bool SetShader(string shaderFileName)
		{
			if (!shaders.TryGetValue(shaderFileName, out shaderCurrent))
			{
				shaderCurrent = shaderDefault;
			}
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));
			if (!shaderCurrent.IsLinked)
			{
				shaderCurrent = shaderDefault;
			}
			shaderCurrent.Activate();
			return shaderCurrent != shaderDefault;
		}

		public void Update()
		{
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));

			glTimer.Activate(QueryTarget.TimeElapsed);
			//texture binding
			int id = 0;
			foreach (var tex in textures)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				tex.Activate();
				GL.Uniform1(shaderCurrent.GetResourceLocation(ShaderResourceType.Uniform, "tex" + id.ToString()), id);
				++id;
			}
			//bind last frame as texture
			var last = surface.Last;
			GL.ActiveTexture(TextureUnit.Texture0 + id);
			last.Activate();
			GL.Uniform1(shaderCurrent.GetResourceLocation(ShaderResourceType.Uniform, "texLastFrame"), id);

			surface.Render();

			id = 0;
			foreach (var tex in textures)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				tex.Deactivate();
				++id;
			}
			//unbind last frame as texture
			GL.ActiveTexture(TextureUnit.Texture0 + id);
			last.Deactivate();
			GL.ActiveTexture(TextureUnit.Texture0);
			glTimer.Deactivate();
		}

		public void UpdateSurfaceSize(int width, int height)
		{
			surface.UpdateSurfaceSize(width, height);
		}

		public bool AddUpdateTexture(string fileName)
		{
			if (File.Exists(fileName))
			{
				try
				{
					var tex = TextureLoader.FromFile(fileName);
					int index = textureNames.FindIndex((string name) => name == fileName);
					if (0 <= index)
					{
						//readd
						textures[index].Dispose();
						textures[index] = tex;
					}
					textureNames.Add(fileName);
					textures.Add(tex);
					return true;
				}
				catch {	}
			}
			return false;
		}

		public IEnumerable<string> GetTextureNames()
		{
			return textureNames;
		}

        public bool RemoveTexture(string fileName)
		{
			int index = textureNames.FindIndex((string name) => name == fileName);
			if(0 <= index)
			{
				textures[index].Dispose();
				textureNames.RemoveAt(index);
				textures.RemoveAt(index);
				return true;
			}
			return false;
		}

		public string AddUpdateFragmentShader(string fileName)
		{
			try
			{
				if (shaders.ContainsKey(fileName))
				{
					if (shaderDefault != shaders[fileName])
					{
						shaders[fileName].Dispose();
						shaders[fileName] = shaderDefault;
					}
				}
				var sFragmentShd = ShaderLoader.ShaderStringFromFileWithIncludes(fileName, false);
				var shader = ShaderLoader.FromStrings(DefaultShader.VertexShaderScreenQuad, sFragmentShd);
				shaders[fileName] = shader;
				return shader.LastLog;
			}
			catch
			{
				try
				{
					var sFragmentShd = ShaderLoader.ShaderStringFromFileWithIncludes(fileName, true);
					var shader = ShaderLoader.FromStrings(DefaultShader.VertexShaderScreenQuad, sFragmentShd);
					shaders[fileName] = shader;
					return shader.LastLog;
				}
				catch (ShaderException e)
				{
					throw new ShaderLoadException(e.Message + Environment.NewLine + e.ShaderLog);
				}
			}
		}

		public void RemoveShader(string shaderFileName)
		{
			if (shaders.TryGetValue(shaderFileName, out IShader shader))
			{
				shader.Dispose();
				shaders.Remove(shaderFileName);
			}
		}

		public void Draw(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			copyToScreen.Draw(surface.Active);
			surface.SwapRenderBuffer();
		}

		public Bitmap GetScreenshot()
		{
			return TextureLoader.SaveToBitmap(surface.Active);
		}

		public float UpdateTime { get { return (float)(glTimer.ResultLong * 1e-9); } }
		//public IEnumerable<string> ShaderList { get { return shaders.Keys; } }
		//public IEnumerable<string> TextureList { get { return textureNames; } }

		private List<string> textureNames = new List<string>();
		private List<ITexture> textures = new List<ITexture>();
		private Dictionary<string, IShader> shaders = new Dictionary<string, IShader>();
		private RenderSurfacePingPong surface;
		private TextureToFrameBuffer copyToScreen;
		private IShader shaderCurrent;
		private IShader shaderDefault;
		private QueryObject glTimer = new QueryObject();

		protected override void DisposeResources()
		{
			foreach (var shader in shaders.Values) shader.Dispose();
			foreach (var tex in textures) tex.Dispose();
			shaderDefault.Dispose();
			copyToScreen.Dispose();
			surface.Dispose();
		}
	}
}
