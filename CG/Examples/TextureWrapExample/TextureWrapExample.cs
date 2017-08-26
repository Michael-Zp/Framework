using DMS.Application;
using DMS.Geometry;
using DMS.HLGL;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	/// <summary>
	/// shows side scrolling setup by manipulating texture coordinates
	/// </summary>
	class MyVisual
	{
		private ITexture texBackground;
		private Box2D texCoord = new Box2D(-1, -1, 3, 3);

		private MyVisual()
		{
			texBackground = TextureLoader.FromBitmap(Resourcen.mountains);
			//background clear color
			GL.ClearColor(Color.Black);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White); //todo student: i) change color
			//draw with different wrap modes - defines how texture coordinates outside of [0..1]² are handled
			texBackground.WrapFunction = TextureWrapFunction.ClampToBorder;
			DrawTexturedRect(new Box2D(-1, 0, 1, 1), texBackground, texCoord);
			texBackground.WrapFunction = TextureWrapFunction.Repeat;
			DrawTexturedRect(new Box2D(0, 0, 1, 1), texBackground, texCoord);
			texBackground.WrapFunction = TextureWrapFunction.ClampToEdge;
			DrawTexturedRect(new Box2D(-1, -1, 1, 1), texBackground, texCoord);
			texBackground.WrapFunction = TextureWrapFunction.MirroredRepeat;
			DrawTexturedRect(new Box2D(0, -1, 1, 1), texBackground, texCoord);
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleWindow();
			var visual = new MyVisual();
			app.Render += visual.Render;
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