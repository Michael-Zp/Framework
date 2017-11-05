using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace MvcSokoban
{
	public class RendererGL4 : IRenderer
	{
		public RendererGL4(IRenderContext context)
		{
			levelVisual = new VisualLevel(context);
			font = new FontGL();
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void DrawLevelState(ILevel levelState, Color tint)
		{
			levelVisual.DrawLevelState(levelState, tint);
		}

		public void Print(string message, float size, TextAlignment alignment = TextAlignment.Left)
		{
			font.Print(message, size, alignment);
		}

		public void ResizeWindow(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			levelVisual.ResizeWindow(width, height);
			font.ResizeWindow(width, height);
		}

		private VisualLevel levelVisual;
		private FontGL font;
	}
}