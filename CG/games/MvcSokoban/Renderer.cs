using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using DMS.HLGL;
using System.Collections.Generic;
using System.Drawing;

namespace MvcSokoban
{
	public class Renderer : IRenderer
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

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void DrawLevel(ILevel level, Color tint)
		{
			GL.Color3(tint);
			GL.LoadIdentity();
			var fitBox = Box2dExtensions.CreateContainingBox(level.Width, level.Height, windowAspect);
			GL.Ortho(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0.0, 1.0);
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
		}

		public void Print(string message, float size, TextAlignment alignment = TextAlignment.Left)
		{
			GL.LoadIdentity();
			GL.Ortho(0, windowAspect, 0, 1, 0, 1);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			var alignmentDelta = 0f;
			switch(alignment)
			{
				case TextAlignment.Right: alignmentDelta = windowAspect - font.Width(message, size); break;
				case TextAlignment.Center: alignmentDelta = .5f * (windowAspect - font.Width(message, size)); break;
			}
			font.Print(alignmentDelta, 0, 0, size, message);
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
