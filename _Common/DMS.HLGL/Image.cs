using DMS.Base;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Numerics;

namespace DMS.HLGL
{
	//todo: move all gl classes into a manager class that handles dispose; do not use gl classes directly
	public class Image : Disposable
	{
		public Image(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false): this(hasDepthBuffer)
		{
			var tex = TextureLoader.Create(width, height, components, floatingPoint);
			if (hasDepthBuffer)
			{
				fbo = new FBOwithDepth(tex);
			}
			else
			{
				fbo = new FBO(tex);
			}
		}

		public Image(bool hasDepthBuffer = false)
		{
			if (ReferenceEquals(null, stateManager))
			{
				stateManager = ContextGL.CreateStateManager();
				stateManager.Get<IStateCommand<Vector4>, States.IClearColor>().Value = Vector4.One;
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

		public Texture Texture { get { return fbo?.Texture; } }

		public void Clear()
		{
			stateManager.Get<StateActiveFboGL, StateActiveFboGL>().Fbo = fbo;
			actionClear();
		}

		public void Draw(DrawConfiguration config)
		{
			stateManager.Get<StateActiveFboGL, StateActiveFboGL>().Fbo = fbo;
			config.Draw(stateManager);
		}

		protected override void DisposeResources()
		{
			if (!ReferenceEquals(null, fbo)) fbo.Dispose();
		}

		private FBO fbo = null;
		private Action actionClear = null;
		private static StateManager stateManager = null;
	}
}
