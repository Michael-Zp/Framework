using DMS.HLGL;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using System;

namespace DMS.OpenGL
{
	//public interface IClearColor : ICommand { };

	public class ContextGL : IContext
	{
		public ContextGL()
		{
			StateManager = new StateManager();
			StateManager.Register<StateActiveFboGL, StateActiveFboGL>(new StateActiveFboGL());
			StateManager.Register<StateActiveShaderGL, StateActiveShaderGL>(new StateActiveShaderGL());
			StateManager.Register<IStateBool, States.IZBufferTest>(new StateBoolGL(EnableCap.DepthTest));
			StateManager.Register<IStateBool, States.IBackfaceCulling>(new StateBoolGL(EnableCap.CullFace));
			StateManager.Register<IStateBool, States.IShaderPointSize>(new StateBoolGL(EnableCap.ProgramPointSize));
			StateManager.Register<IStateBool, States.IPointSprite>(new StateBoolGL(EnableCap.PointSprite));
			StateManager.Register<IStateBool, States.IBlending>(new StateBoolGL(EnableCap.Blend));
			StateManager.Register<IStateTyped<float>, States.ILineWidth>(new StateCommandGL<float>(GL.LineWidth, 1f));
			StateManager.Register<IStateTyped<Vector4>, States.IClearColor>(new StateCommandGL<Vector4>(ClearColor, Vector4.Zero));
			//stateManager.Register<ICommand, IClearColor>(new CommandGL());
			//StateManager.Register<ICreator<IShader>, IShader>(new ShaderCreatorGL());
		}

		public IStateManager StateManager { get; private set; }

		private void ClearColor(Vector4 c) => GL.ClearColor(c.X, c.Y, c.Z, c.W);
		public void ClearColor() => GL.Clear(ClearBufferMask.ColorBufferBit);
		public void ClearColorDepth() => GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		public void DrawPoints(int count)
		{
			GL.DrawArrays(PrimitiveType.Points, 0, count);
		}

		public IImage GetFrameBuffer()
		{
			throw new NotImplementedException();
		}

		public IDrawConfiguration CreateDrawConfiguration()
		{
			throw new NotImplementedException();
		}
	}
}
