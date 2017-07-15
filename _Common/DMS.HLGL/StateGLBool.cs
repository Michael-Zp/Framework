using OpenTK.Graphics.OpenGL;

namespace DMS.HLGL
{
	public class StateGLBool
	{
		public StateGLBool(EnableCap capability)
		{
			this.capability = capability;
			UpdateGL();
		}

		public bool Enabled
		{
			get { return enabled; }

			set
			{
				if (value == enabled) return;
				enabled = value;
				UpdateGL();
			}
		}

		private bool enabled = false;
		private readonly EnableCap capability;

		private void UpdateGL()
		{
			if (enabled)
			{
				GL.Enable(capability);
			}
			else
			{
				GL.Disable(capability);
			}
		}
	}
}
