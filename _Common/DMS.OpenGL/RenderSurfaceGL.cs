using DMS.Base;
using DMS.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;

namespace DMS.OpenGL
{
	//todo: move all gl classes into a manager class that handles dispose; do not use gl classes directly
	public class RenderSurfaceGL : Disposable, IRenderSurface
	{
		public RenderSurfaceGL(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false): this(hasDepthBuffer)
		{
			var tex = OpenGL.Texture2dGL.Create(width, height, components, floatingPoint);
			if (hasDepthBuffer)
			{
				fbo = new FBOwithDepth(tex);
			}
			else
			{
				fbo = new FBO(tex);
			}
		}

		public RenderSurfaceGL(bool hasDepthBuffer = false)
		{
			if (ReferenceEquals(null, context))
			{
				context = new RenderContextGL();
				//context.StateManager.Get<IStateTyped<Vector4>, States.IClearColor>().Value = new Vector4(0, .3f, .7f, 1);
			}

			if (hasDepthBuffer)
			{
				actionClear = () => GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			}
			else
			{
				actionClear = () => GL.Clear(ClearBufferMask.ColorBufferBit);
			}
		}

		public ITexture Texture { get { return fbo?.Texture; } }

		public void Clear()
		{
			context.StateManager.Get<StateActiveFboGL, StateActiveFboGL>().Fbo = fbo;
			actionClear();
		}

		public void Draw(IDrawConfiguration config)
		{
			context.StateManager.Get<StateActiveFboGL, StateActiveFboGL>().Fbo = fbo;
			config.Draw(context);
		}

		protected override void DisposeResources()
		{
			if (!ReferenceEquals(null, fbo)) fbo.Dispose();
		}

		private FBO fbo = null;
		private Action actionClear = null;
		private static RenderContextGL context = null;
	}
}
