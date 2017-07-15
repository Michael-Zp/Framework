using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System;

namespace DMS.HLGL
{
	public class StateSetGL
	{
		public bool GetState<TYPE>() where TYPE : IState
		{
			return boolStates[typeof(TYPE)].Enabled;
		}

		public void SetState<TYPE>(bool value) where TYPE : IState
		{
			boolStates[typeof(TYPE)].Enabled = value;
		}

		public void Register<TYPE>(EnableCap cap) where TYPE : IState
		{
			boolStates.Add(typeof(TYPE), new StateGLBool(cap));
		}

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

		private Dictionary<Type, StateGLBool> boolStates = new Dictionary<Type, StateGLBool>();
		private FBO fbo = null;
		//private VAO vao = null;
		private Shader shader = null;
	}
}
