using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Example
{
	public class MyWindow
	{
		float x = -1;
		float delta = 0.01f;

		public void Update(float updatePeriod)
		{
			//todo: ask for key with Keyboard.GetState()[Key.Left]
			if (x > 0.5f || x < -1) delta = -delta;
			x += delta;
		}

		public void Render()
		{
			//clear screen - what happens without?
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//draw a primitive
			GL.Begin(PrimitiveType.Quads);
			//set color color is active as long as no other color is set
			GL.Color3(Color.Cyan);
			GL.Vertex2(x, 0.0f);
			GL.Color3(Color.Chartreuse);
			GL.Vertex2(x + 0.5f, 0.0f);
			GL.Color3(Color.Coral);
			GL.Vertex2(x + 0.1f, 0.7f);
			GL.Color3(Color.Red);
			GL.Vertex2(x, 0.5f);
			GL.End();
		}
	}
}
