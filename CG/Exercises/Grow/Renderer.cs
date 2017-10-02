using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Example
{
	public class Renderer
	{
		public Renderer()
		{
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void Resize(int width, int height)
		{
			rendererPoints.Resize(width, height);
		}

		public void DrawElements(IEnumerable<IElement> elements)
		{
			//marshall data for gpu
			var coords = new Vector3[elements.Count()];
			int i = 0;
			foreach (var element in elements)
			{
				coords[i] = Convert(element);
				++i;
			}
			rendererPoints.DrawPoints(coords, .05f, Color.CornflowerBlue);
		}

		public void DrawPlayer(IElement element)
		{
			rendererPoints.DrawPoints(new Vector3[] { Convert(element) }, .05f, Color.Red);
		}

		private RendererPoints rendererPoints = new RendererPoints();

		private static Vector3 Convert(IElement element)
		{
			return new Vector3(element.Coord, (1f - element.Size));
		}
	}
}
