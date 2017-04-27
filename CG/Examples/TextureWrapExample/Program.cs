using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	/// <summary>
	/// shows side scrolling setup by manipulating texture coordinates
	/// </summary>
	class MyWindow : IWindow
	{
		private Texture texBackground;
		private Box2D texCoord = new Box2D(-1, -1, 3, 3);

		private MyWindow()
		{
			texBackground = TextureLoader.FromBitmap(Resourcen.mountains);
			//background clear color
			GL.ClearColor(Color.Black);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw with different wrap modes - defines how texture coordinates outside of [0..1]² are handled
			texBackground.WrapMode(TextureWrapMode.ClampToBorder);
			DrawTexturedRect(new Box2D(-1, 0, 1, 1), texBackground, texCoord);
			texBackground.WrapMode(TextureWrapMode.Repeat);
			DrawTexturedRect(new Box2D(0, 0, 1, 1), texBackground, texCoord);
			texBackground.WrapMode(TextureWrapMode.ClampToEdge);
			DrawTexturedRect(new Box2D(-1, -1, 1, 1), texBackground, texCoord);
			texBackground.WrapMode(TextureWrapMode.MirroredRepeat);
			DrawTexturedRect(new Box2D(0, -1, 1, 1), texBackground, texCoord);
		}

		public void Update(float updatePeriod)
		{
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