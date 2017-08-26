using DMS.Base;
using DMS.HLGL;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace MvcSokoban
{
	internal class FontGL
	{
		public FontGL(IRenderContext context)
		{
			shdFont = context.CreateShader();
			//shdFont.FromStrings(Tools.ToString(Resourcen.texColorVert), Tools.ToString(Resourcen.texColorFrag));
			locCamera = shdFont.GetResourceLocation(ShaderResourceType.Uniform, "camera");
		}

		public void Print(string message, float size, TextAlignment alignment = TextAlignment.Left)
		{
			var camera = Matrix4x4.CreateOrthographicOffCenter(0, windowAspect, 0, 1, 0, 1).ToOpenTK();
			GL.UniformMatrix4(locCamera, false, ref camera);
		}

		internal void ResizeWindow(int width, int height)
		{
			windowAspect = width / (float)height;
		}

		private IShader shdFont;
		private float windowAspect;
		private int locCamera;
	}
}