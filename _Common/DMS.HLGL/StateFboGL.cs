using DMS.OpenGL;

namespace DMS.HLGL
{
	public class StateFboGL : IState
	{
		public FBO Fbo
		{
			get { return fbo; }

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
