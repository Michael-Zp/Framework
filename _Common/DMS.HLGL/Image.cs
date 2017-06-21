using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace DMS.HLGL
{
	public class Image
	{
		public Image(int width, int height, bool hasDepthBuffer = false): this(hasDepthBuffer)
		{
			fbo = new FBO(Texture.Create(width, height), hasDepthBuffer);
		}

		public Image(bool hasDepthBuffer = false)
		{
			if (ReferenceEquals(null, stateSetGL)) stateSetGL = new StateSetGL();
			if (hasDepthBuffer)
			{
				actionClear = () => GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			}
			else
			{
				actionClear = () => GL.Clear(ClearBufferMask.ColorBufferBit);
			}
		}

		public Texture Texture { get { return fbo?.Texture; } }

		public void Clear()
		{
			stateSetGL.Fbo = fbo;
			actionClear();
		}

		public void Draw(DrawParameters parameters)
		{
			stateSetGL.Fbo = fbo;
			stateSetGL.BackfaceCulling = parameters.BackfaceCulling;
			stateSetGL.ZBufferTest = parameters.ZBufferTest;
			var shader = parameters.Shader;
			stateSetGL.Shader = shader;
			BindTextures(shader, parameters.Textures);
			//todo: set shader parameters
			var vao = parameters.Vao;
			if (ReferenceEquals(null, vao))
			{
				GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			}
			else
			{
				parameters.Vao.Draw();
			}
			//todo: parameters.Geometry;
			UnbindTextures(parameters.Textures);
		}

		private FBO fbo = null;
		private Action actionClear = null;
		private static StateSetGL stateSetGL = null;

		private void BindTextures(Shader shader, List<NamedTexture> textures)
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

		private void UnbindTextures(List<NamedTexture> textures)
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
