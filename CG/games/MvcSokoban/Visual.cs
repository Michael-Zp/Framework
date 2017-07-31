using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using DMS.HLGL;
using System.Collections.Generic;

namespace MvcSokoban
{
	class Visual
	{
		public Visual()
		{
			tileSet.Add(ElementType.Floor, TextureLoader.FromBitmap(Resourcen.GroundGravel_Grass));
			tileSet.Add(ElementType.Man, TextureLoader.FromBitmap(Resourcen.Character4));
			tileSet.Add(ElementType.Box, TextureLoader.FromBitmap(Resourcen.Crate_Brown));
			tileSet.Add(ElementType.Goal, TextureLoader.FromBitmap(Resourcen.EndPoint_Red));
			tileSet.Add(ElementType.ManOnGoal, TextureLoader.FromBitmap(Resourcen.EndPointCharacter));
			tileSet.Add(ElementType.BoxOnGoal, TextureLoader.FromBitmap(Resourcen.EndPointCrate_Brown));
			tileSet.Add(ElementType.Wall, TextureLoader.FromBitmap(Resourcen.Wall_Beige));

			//this.spriteSheet = new SpriteSheet(TextureLoader.FromBitmap(Resourcen.skin36), 4, 0.98f, 0.97f);
			this.spriteSheet = new SpriteSheet(TextureLoader.FromBitmap(Resourcen.borgar), 4, 0.95f, 0.95f);
		}

		public void DrawScreen(ILevel level)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			var fitBox = Box2dExtensions.CreateFitBox(level.Width, level.Height, windowWidth / (float)windowHeight);
			GL.Ortho(fitBox.X, fitBox.MaxX, fitBox.Y, fitBox.MaxY, 0.0, 1.0);
			GL.MatrixMode(MatrixMode.Modelview);

			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//spriteSheet.Activate();
			GL.Color3(Color.White);
			for (int x = 0; x < level.Width; ++x)
			{
				for (int y = 0; y < level.Height; ++y)
				{
					var rect = new Box2D(x, y, 1.0f, 1.0f);
					//spriteSheet.Draw((uint)level.GetElement(x, y), rect);
					var element = level.GetElement(x, y);
					var tex = tileSet[element];
					tex.Activate();
					rect.DrawTexturedRect(Box2D.BOX01);
					tex.Deactivate();
				}
			}

			//spriteSheet.Deactivate();
			GL.Disable(EnableCap.Blend);
		}

		public void ResizeWindow(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			windowWidth = width;
			windowHeight = height;
		}

		private readonly SpriteSheet spriteSheet;
		private Dictionary<ElementType, ITexture> tileSet = new Dictionary<ElementType, ITexture>();
		private int windowWidth;
		private int windowHeight;
	}
}
