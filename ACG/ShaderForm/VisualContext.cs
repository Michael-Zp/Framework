using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ShaderForm
{
	[Serializable]
	public class ShaderLoadException : Exception
	{
		public ShaderLoadException(string msg) : base(msg) { }
	}

	public class VisualContext : IDisposable, ISetUniform
	{
		public VisualContext()
		{
			GL.Disable(EnableCap.DepthTest);
			GL.ClearColor(1, 0, 0, 0);

			surface = new FBO();
			textureBufferA = CreateTexture(1, 1);
			textureBufferB = CreateTexture(1, 1);
			active = textureBufferA;

			shaderCopyToScreen = InitShaderCopyToScreen();
			shaderDefault = InitShaderDefault();
		}

		public void Dispose()
		{
			foreach (var shader in shaders.Values) shader.Dispose();
			foreach (var tex in textures) tex.Dispose();
			shaderDefault.Dispose();
			shaderCopyToScreen.Dispose();
			active = null;
			textureBufferB.Dispose();
			textureBufferA.Dispose();
			surface.Dispose();
		}

		public void SetUniform(string uniformName, float value)
		{
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));
			GL.Uniform1(shaderCurrent.GetUniformLocation(uniformName), value);
		}

		public void SetUniform(string uniformName, float valueX, float valueY)
		{
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));
			GL.Uniform2(shaderCurrent.GetUniformLocation(uniformName), valueX, valueY);
		}

		public void SetUniform(string uniformName, float valueX, float valueY, float valueZ)
		{
			Debug.Assert(!ReferenceEquals(null,  shaderCurrent));
			GL.Uniform3(shaderCurrent.GetUniformLocation(uniformName), valueX, valueY, valueZ);
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

			//texture binding
			int id = 0;
			foreach (var tex in textures)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				tex.Activate();
				GL.Uniform1(shaderCurrent.GetUniformLocation("tex" + id.ToString()), id);
				++id;
			}
			//bind last frame as texture
			var last = (active == textureBufferA) ? textureBufferB : textureBufferA;
			GL.ActiveTexture(TextureUnit.Texture0 + id);
			last.Activate();
			GL.Uniform1(shaderCurrent.GetUniformLocation("texLastFrame"), id);

			surface.Activate(active);
			GL.Viewport(0, 0, active.Width, active.Height);

			//Drawing
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);

			surface.Deactivate();

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
		}

		public void UpdateSurfaceSize(int width, int height)
		{
			if (0 == width || 0 == height) return;
			if (width != active.Width || height != active.Height)
			{
				var isTexAactive = active == textureBufferA;
				textureBufferB.Dispose();
				textureBufferA.Dispose();
				textureBufferA = CreateTexture(width, height);
				textureBufferB = CreateTexture(width, height);
				active = isTexAactive ? textureBufferA : textureBufferB;
			}
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
			string sVertexShader = @"
				#version 130				
				uniform vec2 iResolution;
				varying vec2 uv;
				out vec2 fragCoord;
				void main() {
					const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0),
                                    vec2( 1.0, -1.0),
                                    vec2( 1.0,  1.0),
                                    vec2(-1.0,  1.0));
					vec2 pos = vertices[gl_VertexID];
					uv = pos * 0.5 + 0.5;
					fragCoord = uv * iResolution;
					gl_Position = vec4(pos, 0.0, 1.0);
				}";

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
				var shader = ShaderLoader.FromStrings(sVertexShader, sFragmentShd);
				shaders[fileName] = shader;
				return shader.LastLog;
			}
			catch
			{
				try
				{
					var sFragmentShd = ShaderLoader.ShaderStringFromFileWithIncludes(fileName, true);
					var shader = ShaderLoader.FromStrings(sVertexShader, sFragmentShd);
					shaders[fileName] = shader;
					return shader.LastLog;
				}
				catch (ShaderException e)
				{
					throw new ShaderLoadException(e.Type + " failed!" + Environment.NewLine + e.Log);
				}
			}
		}

		public void RemoveShader(string shaderFileName)
		{
			Shader shader;
			if (shaders.TryGetValue(shaderFileName, out shader))
			{
				shader.Dispose();
				shaders.Remove(shaderFileName);
			}
		}

		public void Draw(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			active.Activate();
			shaderCopyToScreen.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderCopyToScreen.Deactivate();
			active.Deactivate();
			var last = (active == textureBufferA) ? textureBufferB : textureBufferA;
			active = last;
		}

		public void Save(string fileName)
		{
			TextureLoader.SaveToFile(active, fileName);
		}

		//public IEnumerable<string> ShaderList { get { return shaders.Keys; } }
		//public IEnumerable<string> TextureList { get { return textureNames; } }

		private List<string> textureNames = new List<string>();
		private List<Texture> textures = new List<Texture>();
		private Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();
		private FBO surface;
		private Texture textureBufferA, textureBufferB, active;
		private Shader shaderCopyToScreen;
		private Shader shaderCurrent;
		private Shader shaderDefault;

		private Texture CreateTexture(int width, int height)
		{
			//return Texture.Create(width, height);
			//return Texture.Create(width, height, PixelInternalFormat.Rgba16f, PixelFormat.Rgba, PixelType.HalfFloat);
			return Texture.Create(width, height, PixelInternalFormat.Rgba32f, PixelFormat.Rgba, PixelType.Float);
		}

		private Shader InitShaderDefault()
		{
			string sVertexShader = @"
				#version 130				
				uniform vec2 iResolution;
				varying vec2 uv;
				out vec2 fragCoord;
				void main() {
					const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0),
                                    vec2( 1.0, -1.0),
                                    vec2( 1.0,  1.0),
                                    vec2(-1.0,  1.0));
					vec2 pos = vertices[gl_VertexID];
					uv = pos * 0.5 + 0.5;
					fragCoord = uv * iResolution;
					gl_Position = vec4(pos, 0.0, 1.0);
				}";
			string sFragmentShd = @"
			varying vec2 uv;
			void main() {
				vec2 uv10 = floor(uv * 10.0f);
				if(1.0 > mod(uv10.x + uv10.y, 2.0f))
					discard;		
				gl_FragColor = vec4(1, 1, 0, 0);
			}";
			return ShaderLoader.FromStrings(sVertexShader, sFragmentShd);
		}

		private Shader InitShaderCopyToScreen()
		{
			string sVertexShader = @"
				#version 430 core				
				out vec2 uv; 
				void main() {
					const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0),
                                    vec2( 1.0, -1.0),
                                    vec2( 1.0,  1.0),
                                    vec2(-1.0,  1.0));
					vec2 pos = vertices[gl_VertexID];
					uv = pos * 0.5 + 0.5;
					gl_Position = vec4(vertices[gl_VertexID], 1.0, 1.0);
				}";
			string sFragmentShd = @"
			varying vec2 uv;
			uniform sampler2D tex;
			void main() {
				gl_FragColor = texture(tex, uv);
			}";
			return ShaderLoader.FromStrings(sVertexShader, sFragmentShd);
		}
	}
}
