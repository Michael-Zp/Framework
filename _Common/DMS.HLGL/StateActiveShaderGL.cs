using DMS.OpenGL;

namespace DMS.HLGL
{
	public class StateActiveShaderGL : IState
	{
		public Shader Shader
		{
			get { return shader; }

			set
			{
				if (ReferenceEquals(shader, value)) return;
				shader?.Deactivate();
				shader = value;
				shader?.Activate();
			}
		}

		private Shader shader = null;
	}
}
