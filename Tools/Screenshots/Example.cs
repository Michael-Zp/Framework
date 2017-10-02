using Zenseless.Application;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel.Composition;
using System.Drawing;

namespace Screenshots
{
	[Export(typeof(IExample))]
	class Example : IExample
	{
		public Example()
		{
			GL.ClearColor(Color.CornflowerBlue);
		}

		public void Update()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//draw a quad
			GL.Begin(PrimitiveType.Quads);
			//color is active as long as no new color is set
			GL.Color3(Color.Cyan);
			GL.Vertex2(0.0f, 0.0f); //draw first quad corner
			GL.Vertex2(0.5f, 0.0f);
			GL.Color3(Color.White);
			GL.Vertex2(0.5f, 0.5f);
			GL.Vertex2(0.0f, 0.5f);
			GL.End();
		}
	}
}
