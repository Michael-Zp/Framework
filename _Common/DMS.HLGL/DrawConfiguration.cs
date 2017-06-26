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

		public void Draw(StateSetGL stateSetGL)
		{
			stateSetGL.BackfaceCulling = BackfaceCulling;
			stateSetGL.ZBufferTest = ZBufferTest;
			var shader = Shader;
			stateSetGL.Shader = shader;
			//todo: set shader parameters

			BindTextures(shader, textures);

			var vao = Vao;
			//todo: parameters.Geometry;
			var bindingIndex = shader.GetUniformBufferBindingIndex("bufferMaterials");
			parameterBuffer.ActivateBind(bindingIndex);
			if (ReferenceEquals(null, vao))
			{
				GL.DrawArrays(PrimitiveType.Quads, 0, 4); //todo: make this general -> mesh with only vertex count?
			}
			else
			{
				Vao.Draw();
			}
			parameterBuffer.Deactivate();
			UnbindTextures(textures);

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
			parameterBuffer.Set(parameters, BufferUsageHint.StaticRead);
		}

		private List<NamedTexture> textures = new List<NamedTexture>();
		private BufferObject parameterBuffer = new BufferObject(BufferTarget.UniformBuffer);

		private static void BindTextures(Shader shader, List<NamedTexture> textures)
		{
			int id = 0;
			if (ReferenceEquals(null, shader))
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
					GL.Uniform1(shader.GetUniformLocation(namedTex.Name), id);
					++id;
				}
			}
		}

		private static void UnbindTextures(List<NamedTexture> textures)
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