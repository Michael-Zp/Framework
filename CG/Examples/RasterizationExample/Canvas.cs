using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Example
{
	public class Canvas
	{
		public Canvas()
		{
			GL.ClearColor(Color.White);
		}

		public void Draw()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.Color3(0.2, 0.4, 1);
			GL.Begin(PrimitiveType.Lines);
			GL.Vertex2(-1, -1);
			GL.Vertex2(1, 1);
			GL.End();
		}
	}
}
