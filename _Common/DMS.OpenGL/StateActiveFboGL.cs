using DMS.HLGL;

namespace DMS.OpenGL
{
	public class StateActiveFboGL : IState
	{
		public FBO Fbo
		{
			get => fbo;
			set
			{
				if (ReferenceEquals(fbo, value)) return;
				fbo?.Deactivate();
				fbo = value;
				fbo?.Activate();
			}
		}

		private FBO fbo = null;
	}
}
