using DMS.HLGL;

namespace DMS.OpenGL
{
	public class StateActiveShaderGL : IState
	{
		public Shader Shader
		{
			get => shader;
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
