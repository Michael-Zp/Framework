using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	/// <summary>
	/// Compares texture magnification filter methods
	/// </summary>
	class MyWindow : IWindow
	{
		private Texture texBackgroundNearest;
		private Texture texBackgroundLinear;
		private Box2D texCoord = new Box2D(0, 0, 1, 1);
		private float scaleFactor = 1f;

		private MyWindow()
		{
			texBackgroundNearest = TextureLoader.FromBitmap(Resourcen.mountains);
			texBackgroundNearest.FilterNearest(); //filter by taking the nearest texel's color as a pixels color
			texBackgroundNearest.WrapMode(TextureWrapMode.ClampToBorder);
			texBackgroundLinear = TextureLoader.FromBitmap(Resourcen.mountains);
			texBackgroundLinear.FilterLinear(); //filter by calculating the pixels color as an weighted average of the neighboring texel's colors
			texBackgroundLinear.WrapMode(TextureWrapMode.ClampToBorder);
			//background clear color
			GL.ClearColor(Color.Black);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw with different filter modes - defines how texture colors are mapped to pixel colors
			DrawTexturedRect(new Box2D(-1, -1, 1, 2), texBackgroundNearest, texCoord);
			DrawTexturedRect(new Box2D(0, -1, 1, 2), texBackgroundLinear, texCoord);
		}

		public void Update(float updatePeriod)
		{
			if (texCoord.SizeX > 0.99f || texCoord.SizeX < 0.05f) scaleFactor = -scaleFactor;
			float factor = 1 + scaleFactor * updatePeriod;
			texCoord.SizeX *= factor;
			texCoord.SizeY *= factor;
			texCoord.CenterX = 0.5f;
			texCoord.CenterY = 0.5f;
		}

		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}

		private static void DrawTexturedRect(Box2D rect, Texture tex, Box2D texCoords)
		{
			tex.Activate();
			rect.DrawTexturedRect(texCoords);
			tex.Deactivate();
		}
	}
}