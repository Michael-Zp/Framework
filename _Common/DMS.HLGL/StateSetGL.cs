using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace DMS.HLGL
{
	public class StateSetGL
	{
		public bool BackfaceCulling
		{
			get	{ return backfaceCulling.Enabled; }
			set { backfaceCulling.Enabled = value; }
		}

		public bool ShaderPointSize
		{
			get { return shaderPointSize.Enabled; }
			set { shaderPointSize.Enabled = value; }
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

		public bool ZBufferTest
		{
			get { return zBufferTest.Enabled; }
			set { zBufferTest.Enabled = value; }
		}

		private StateGLBool backfaceCulling = new StateGLBool(EnableCap.CullFace);
		private FBO fbo = null;
		//private VAO vao = null;
		private Shader shader = null;
		private StateGLBool shaderPointSize = new StateGLBool(EnableCap.VertexProgramPointSize);
		private StateGLBool zBufferTest = new StateGLBool(EnableCap.DepthTest);
	}
}
