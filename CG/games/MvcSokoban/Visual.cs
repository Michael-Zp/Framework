using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MvcSokoban
{
	class Visual
	{
		public Visual()
		{
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.GroundGravel_Grass));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.Character4));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.Crate_Brown));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.Crate_Brown));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.EndPoint_Red));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.EndPointCharacter));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.Crate_Brown));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.EndPointCrate_Brown));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.Wall_Beige));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.Wall_Beige));
			tileSet.AddFrame(TextureLoader.FromBitmap(Resourcen.Wall_Beige));

			//this.spriteSheet = new SpriteSheet(TextureLoader.FromBitmap(Resourcen.skin36), 4, 0.98f, 0.97f);

			this.spriteSheet = new SpriteSheet(TextureLoader.FromBitmap(Resourcen.borgar), 4, 0.95f, 0.95f);
		}

		public void DrawScreen(ILevel level)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			var levelAspect = level.Width / (float)level.Height;
			var windowAspect = windowWidth / (float)windowHeight;
			var ratio = levelAspect / windowAspect;
			if (levelAspect > windowAspect)
			{
				var delta = level.Height * 0.5f;
				GL.Ortho(0.0, level.Width, 0, level.Height * ratio, 0.0, 1.0);
			}
			else
			{
				GL.Ortho(0.0, level.Width / ratio, 0.0, level.Height, 0.0, 1.0);
			}
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
					var id = (int)level.GetElement(x, y);
					var tex = tileSet.Textures[id];
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
		private readonly AnimationTextures tileSet = new AnimationTextures(5);
		private int windowWidth;
		private int windowHeight;
	}
}
