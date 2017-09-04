using Zenseless.Application;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	/// <summary>
	/// Compares texture magnification filter methods
	/// </summary>
	class MyVisual
	{
		private ITexture texBackgroundNearest;
		private ITexture texBackgroundLinear;
		private Box2D texCoord = new Box2D(0, 0, 1, 1);
		private float scaleFactor = 1f;

		private MyVisual()
		{
			texBackgroundNearest = TextureLoader.FromBitmap(Resourcen.mountains);
			texBackgroundNearest.Filter = TextureFilterMode.Nearest; //filter by taking the nearest texel's color as a pixels color
			texBackgroundNearest.WrapFunction = TextureWrapFunction.ClampToBorder;
			texBackgroundLinear = TextureLoader.FromBitmap(Resourcen.mountains);
			texBackgroundLinear.Filter = TextureFilterMode.Linear; //filter by calculating the pixels color as an weighted average of the neighboring texel's colors
			texBackgroundLinear.WrapFunction = TextureWrapFunction.ClampToBorder;
			//background clear color
			GL.ClearColor(Color.Black);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw with different filter modes - defines how texture colors are mapped to pixel colors
			DrawTexturedRect(new Box2D(-1, -1, 1, 2), texBackgroundNearest, texCoord);
			DrawTexturedRect(new Box2D(0, -1, 1, 2), texBackgroundLinear, texCoord);
		}

		private void Update(float updatePeriod)
		{
			if (texCoord.SizeX > 0.99f || texCoord.SizeX < 0.05f) scaleFactor = -scaleFactor;
			float factor = 1 + scaleFactor * updatePeriod;
			texCoord.SizeX *= factor;
			texCoord.SizeY *= factor;
			texCoord.CenterX = 0.5f;
			texCoord.CenterY = 0.5f;
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			var visual = new MyVisual();
			app.Render += visual.Render;
			app.Update += visual.Update;
			app.Run();
		}

		private static void DrawTexturedRect(Box2D rect, ITexture tex, Box2D texCoords)
		{
			tex.Activate();
			rect.DrawTexturedRect(texCoords);
			tex.Deactivate();
		}
	}
}