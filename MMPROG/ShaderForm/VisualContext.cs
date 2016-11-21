using Framework;
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
			// Vertex positions
			float[] positions =
			{
				1.0f, -1.0f,
				1.0f, 1.0f,
				-1.0f, -1.0f,
				-1.0f, 1.0f
			};
			// Reserve a name for the buffer object.
			bufferQuad = GL.GenBuffer();
			// Bind it to the GL_ARRAY_BUFFER target.
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferQuad);
			// Allocate space for it (sizeof(positions)
			GL.BufferData(BufferTarget.ArrayBuffer
				, (IntPtr)(sizeof(float) * positions.Length), positions, BufferUsageHint.StaticDraw);

			GL.BindVertexArray(bufferQuad);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexPointer(2, VertexPointerType.Float, 0, 0);
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.Disable(EnableCap.DepthTest);
			GL.ClearColor(1, 0, 0, 0);

			surface = new FBO();
			textureSurface = Texture.Create(1, 1);

			shaderCopyToScreen = InitShaderCopyToScreen();
			shaderDefault = InitShaderDefault();
		}

		public void Dispose()
		{
			foreach (var shader in shaders.Values) shader.Dispose();
			foreach (var tex in textures) tex.Dispose();
			shaderDefault.Dispose();
			shaderCopyToScreen.Dispose();
			textureSurface.Dispose();
			surface.Dispose();
		}

		public void SetUniform(string uniformName, float value)
		{
			Debug.Assert(null != shaderCurrent);
			GL.Uniform1(shaderCurrent.GetUniformLocation(uniformName), value);
		}

		public void SetUniform(string uniformName, float valueX, float valueY)
		{
			Debug.Assert(null != shaderCurrent);
			GL.Uniform2(shaderCurrent.GetUniformLocation(uniformName), valueX, valueY);
		}

		public void SetUniform(string uniformName, float valueX, float valueY, float valueZ)
		{
			Debug.Assert(null != shaderCurrent);
			GL.Uniform3(shaderCurrent.GetUniformLocation(uniformName), valueX, valueY, valueZ);
		}

		public bool SetShader(string shaderFileName)
		{
			if (!shaders.TryGetValue(shaderFileName, out shaderCurrent))
			{
				shaderCurrent = shaderDefault;
			}
			Debug.Assert(null != shaderCurrent);
			if (!shaderCurrent.IsLinked)
			{
				shaderCurrent = shaderDefault;
			}
			shaderCurrent.Begin();
			return shaderCurrent != shaderDefault;
		}

		public void Update()
		{
			Debug.Assert(null != shaderCurrent);
			//texture binding
			int id = 0;
			foreach (var tex in textures)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				tex.BeginUse();
				GL.Uniform1(shaderCurrent.GetUniformLocation("tex" + id.ToString()), id);
				++id;
			}
			surface.BeginUse(textureSurface);
			GL.Viewport(0, 0, textureSurface.Width, textureSurface.Height);

			//Drawing
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.BindVertexArray(bufferQuad);
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
			GL.BindVertexArray(0);

			surface.EndUse();

			id = 0;
			foreach (var tex in textures)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				tex.EndUse();
				++id;
			}
			GL.ActiveTexture(TextureUnit.Texture0);
		}

		public void UpdateSurfaceSize(int width, int height)
		{
			if (0 == width || 0 == height) return;
			if (width != textureSurface.Width || height != textureSurface.Height)
			{
				textureSurface.Dispose();
				textureSurface = Texture.Create(width, height);
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
					gl_Position = gl_Vertex;
					uv = gl_Vertex.xy * 0.5f + 0.5f;
					fragCoord = uv * iResolution;
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
			textureSurface.BeginUse();
			shaderCopyToScreen.Begin();
			GL.BindVertexArray(bufferQuad);
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
			GL.BindVertexArray(0);
			shaderCopyToScreen.End();
			textureSurface.EndUse();
		}

		public void Save(string fileName)
		{
			TextureLoader.SaveToFile(textureSurface, fileName);
		}

		//public IEnumerable<string> ShaderList { get { return shaders.Keys; } }
		//public IEnumerable<string> TextureList { get { return textureNames; } }

		private int bufferQuad;
		private List<string> textureNames = new List<string>();
		private List<Texture> textures = new List<Texture>();
		private Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();
		private FBO surface;
		private Texture textureSurface;
		private Shader shaderCopyToScreen;
		private Shader shaderCurrent;
		private Shader shaderDefault;

		private Shader InitShaderDefault()
		{
			string sVertexShader = @"
				#version 130				
				uniform vec2 iResolution;
				varying vec2 uv;
				out vec2 fragCoord;
				void main() {
					gl_Position = gl_Vertex;
					uv = gl_Vertex.xy * 0.5f + 0.5f;
					fragCoord = uv * iResolution;
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
				varying vec2 uv;
				void main() {
					gl_Position = gl_Vertex;
					uv = gl_Vertex.xy * 0.5f + 0.5f;
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
