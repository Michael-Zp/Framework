using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;

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

		public void Draw(Configuration parameters)
		{
			stateSetGL.Fbo = fbo;
			parameters.Draw(stateSetGL);
		}

		private FBO fbo = null;
		private Action actionClear = null;
		private static StateSetGL stateSetGL = null;
	}
}
