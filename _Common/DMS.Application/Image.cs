using DMS.Base;
using DMS.HLGL;
using DMS.OpenGL;
using System;

namespace DMS.Application
{
	//todo: move all gl classes into a manager class that handles dispose; do not use gl classes directly
	public class Image : Disposable
	{
		public Image(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false): this(hasDepthBuffer)
		{
			var tex = Texture.Create(width, height, components, floatingPoint);
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
			if (ReferenceEquals(null, context))
			{
				context = new ContextGL();
				//context.StateManager.Get<IStateTyped<Vector4>, States.IClearColor>().Value = new Vector4(0, .3f, .7f, 1);
			}

			if (hasDepthBuffer)
			{
				actionClear = () => context.ClearColorDepth();
			}
			else
			{
				actionClear = () => context.ClearColor();
			}
		}

		public IContext Context { get { return context; } }
		public Texture Texture { get { return fbo?.Texture; } }

		public void Clear()
		{
			context.StateManager.Get<StateActiveFboGL, StateActiveFboGL>().Fbo = fbo;
			actionClear();
		}

		public void Draw(DrawConfiguration config)
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
		private static ContextGL context = null;
	}
}
