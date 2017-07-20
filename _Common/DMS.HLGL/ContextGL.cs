using OpenTK.Graphics.OpenGL;
using System.Numerics;

namespace DMS.HLGL
{
	public static class ContextGL
	{
		public static StateManager CreateStateManager()
		{
			var stateManager = new StateManager();
			stateManager.Register<StateActiveFboGL, StateActiveFboGL>(new StateActiveFboGL());
			stateManager.Register<StateActiveShaderGL, StateActiveShaderGL>(new StateActiveShaderGL());
			stateManager.Register<IStateBool, States.IZBufferTest>(new StateBoolGL(EnableCap.DepthTest));
			stateManager.Register<IStateBool, States.IBackfaceCulling>(new StateBoolGL(EnableCap.CullFace));
			stateManager.Register<IStateBool, States.IShaderPointSize>(new StateBoolGL(EnableCap.ProgramPointSize));
			stateManager.Register<IStateBool, States.IPointSprite>(new StateBoolGL(EnableCap.PointSprite));
			stateManager.Register<IStateBool, States.IBlending>(new StateBoolGL(EnableCap.Blend));
			stateManager.Register<IStateCommand<float>, States.ILineWidth>(new StateCommandGL<float>(GL.LineWidth, 1f));
			stateManager.Register<IStateCommand<Vector4>, States.IClearColor>(new StateCommandGL<Vector4>(ClearColor, Vector4.Zero));
			return stateManager;
		}

		private static void ClearColor(Vector4 c) => GL.ClearColor(c.X, c.Y, c.Z, c.W); 
	}
}
