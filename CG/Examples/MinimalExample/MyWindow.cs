using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Example
{
	public class MyWindow
	{
		public void Update(float updatePeriod)
		{
			//todo: ask for key with Keyboard.GetState()[Key.Left]
		}

		public void Render()
		{
			//clear screen - what happens without?
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//draw a primitive
			GL.Begin(PrimitiveType.Quads);
			//set color color is active as long as no other color is set
			GL.Color3(Color.White);
			GL.Vertex2(0.0f, 0.0f);
			GL.Vertex2(0.5f, 0.0f);
			GL.Vertex2(0.5f, 0.5f);
			GL.Vertex2(0.0f, 0.5f);
			GL.End();
		}
	}
}
