using DMS.Application;
using DMS.Geometry;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace DMS.HLGL
{
	public class NamedTexture
	{
		public NamedTexture(string name, Texture texture)
		{
			Texture = texture;
			Name = name;
		}

		public string Name { get; private set; }
		public Texture Texture { get; private set; }
	}

	//todo: make this to a node with typed inputs and outputs
	public class DrawConfiguration
	{
		public bool BackfaceCulling { get; set; }
		//todo parameters: buffers
		public Shader Shader { get; private set; }
		public VAO Vao { get; private set; }
		public bool ZBufferTest { get; set; }

		public void AddTexture(NamedTexture tex)
		{
			textures.Add(tex);
		}

		public void AddTexture(string name)
		{
			textures.Add(new NamedTexture(name, ResourceManager.Instance.Get<Texture>(name).Value));
		}

		public void Draw(StateSetGL stateSetGL, int instanceCount = 1)
		{
			stateSetGL.BackfaceCulling = BackfaceCulling;
			stateSetGL.ZBufferTest = ZBufferTest;
			stateSetGL.Shader = Shader;

			BindTextures();
			var hasParameters = !string.IsNullOrEmpty(parameterTypeName);
			if (hasParameters)
			{
				var bindingIndex = Shader.GetUniformBufferBindingIndex(parameterTypeName);
				if (-1 == bindingIndex) throw new ArgumentException("Could not find shader parameters '" + parameterTypeName + "'");
				parameterBuffer.ActivateBind(bindingIndex);
			}

			var vao = Vao;
			//todo: parameters.Geometry;
			if (ReferenceEquals(null, vao))
			{
				GL.DrawArrays(PrimitiveType.Quads, 0, 4); //todo: make this general -> mesh with only vertex count? particle system, sprites
			}
			else
			{
				Vao.Draw();
			}

			if (hasParameters)
			{
				parameterBuffer.Deactivate();
			}
			UnbindTextures();

		}

		public void UpdateMeshShader(Mesh mesh, string shaderName)
		{
			if (string.IsNullOrWhiteSpace(shaderName)) throw new ArgumentException("A shaderName is required");
			var resShader = ResourceManager.Instance.Get<Shader>(shaderName);
			if (ReferenceEquals(null, resShader)) throw new ArgumentException("Shader '" + shaderName + "' does not exist");
			Shader = resShader.Value;
			//if (ReferenceEquals(null, mesh)) throw new ArgumentException("A mesh is required");
			Vao = ReferenceEquals(null, mesh) ? null : VAOLoader.FromMesh(mesh, Shader);
		}

		public void UpdateParameters<DATA>(DATA parameters) where DATA : struct
		{
			var type = typeof(DATA);
			parameterTypeName = type.Name;
			parameterBuffer.Set(parameters, BufferUsageHint.StaticRead);
		}

		private List<NamedTexture> textures = new List<NamedTexture>();
		private BufferObject parameterBuffer = new BufferObject(BufferTarget.UniformBuffer);
		private string parameterTypeName;

		private void BindTextures()
		{
			int id = 0;
			if (ReferenceEquals(null, Shader))
			{
				foreach (var namedTex in textures)
				{
					GL.ActiveTexture(TextureUnit.Texture0 + id);
					namedTex.Texture.Activate();
					++id;
				}
			}
			else
			{
				foreach (var namedTex in textures)
				{
					GL.ActiveTexture(TextureUnit.Texture0 + id);
					namedTex.Texture.Activate();
					GL.Uniform1(Shader.GetUniformLocation(namedTex.Name), id);
					++id;
				}
			}
		}

		private void UnbindTextures()
		{
			int id = 0;
			foreach (var namedTex in textures)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				namedTex.Texture.Deactivate();
				++id;
			}
			GL.ActiveTexture(TextureUnit.Texture0);
		}
	}
}