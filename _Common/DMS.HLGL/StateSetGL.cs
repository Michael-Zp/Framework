using DMS.OpenGL;
using System.Collections.Generic;
using System;

namespace DMS.HLGL
{
	public class StateSetGL
	{
		public StateManager StateManager { get; private set; } = new StateManager();

		public FBO Fbo
		{
			get	{ return fbo; }

			set
			{
				if (ReferenceEquals(fbo, value)) return;
				fbo?.Deactivate();
				fbo = value;
				fbo?.Activate();
			}
		}

		//public VAO Vao
		//{
		//	get	{ return vao; }

		//	set
		//	{
		//		if (ReferenceEquals(vao, value)) return;
		//		vao?.Deactivate();
		//		vao = value;
		//		vao?.Activate();
		//	}
		//}

		public Shader Shader
		{
			get	{ return shader; }

			set
			{
				if (ReferenceEquals(shader, value)) return;
				shader?.Deactivate();
				shader = value;
				shader?.Activate();
			}
		}

		private FBO fbo = null;
		//private VAO vao = null;
		private Shader shader = null;
	}
}
