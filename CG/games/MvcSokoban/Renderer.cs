using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using DMS.HLGL;
using System.Collections.Generic;

namespace MvcSokoban
{
	class Renderer
	{
		public Renderer()
		{
			font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Video_Phreak), 10, 32, 1, 1, .7f);

			tileSet.Add(ElementType.Floor, TextureLoader.FromBitmap(Resourcen.GroundGravel_Grass));
			tileSet.Add(ElementType.Man, TextureLoader.FromBitmap(Resourcen.Character4));
			tileSet.Add(ElementType.Box, TextureLoader.FromBitmap(Resourcen.Crate_Brown));
			tileSet.Add(ElementType.Goal, TextureLoader.FromBitmap(Resourcen.EndPoint_Red));
			tileSet.Add(ElementType.ManOnGoal, TextureLoader.FromBitmap(Resourcen.EndPointCharacter));
			tileSet.Add(ElementType.BoxOnGoal, TextureLoader.FromBitmap(Resourcen.EndPointCrate_Brown));
			tileSet.Add(ElementType.Wall, TextureLoader.FromBitmap(Resourcen.Wall_Beige));

			//this.spriteSheet = new SpriteSheet(TextureLoader.FromBitmap(Resourcen.skin36), 4, 0.98f, 0.97f);
			//this.spriteSheet = new SpriteSheet(TextureLoader.FromBitmap(Resourcen.borgar), 4, 0.95f, 0.95f);
		}

		public void DrawScreen(ILevel level, string message)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			var fitBox = Box2dExtensions.CreateContainingBox(level.Width, level.Height, windowAspect);
			GL.Ortho(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0.0, 1.0);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//spriteSheet.Activate();
			for (int x = 0; x < level.Width; ++x)
			{
				for (int y = 0; y < level.Height; ++y)
				{
					var tile = new Box2D(x, y, 1.0f, 1.0f);
					//spriteSheet.Draw((uint)level.GetElement(x, y), rect);
					var element = level.GetElement(x, y);
					var tex = tileSet[element];
					tex.Activate();
					tile.DrawTexturedRect(Box2D.BOX01);
					tex.Deactivate();
				}
			}
			//spriteSheet.Deactivate();
			var size = 1f;
			var rightjustifiedDelta = level.Width - font.Width(message, size) - .5f * size;
			font.Print(rightjustifiedDelta, 0, 0, size, message);
			GL.Disable(EnableCap.Blend);
		}

		public void ResizeWindow(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			windowAspect = width / (float)height;
		}

		//private readonly SpriteSheet spriteSheet;
		private TextureFont font;
		private Dictionary<ElementType, ITexture> tileSet = new Dictionary<ElementType, ITexture>();
		private float windowAspect;
	}
}
