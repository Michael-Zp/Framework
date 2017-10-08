using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	public class SpriteSheetRenderer
	{
		public SpriteSheetRenderer(ITexture texture, SpriteSheet spriteSheet)
		{
			SpriteSheet = spriteSheet;
			Texture = texture;
			Texture.Filter = TextureFilterMode.Mipmap;
		}
		public void Activate()
		{
			Texture.Activate();
		}

		public void Deactivate()
		{
			Texture.Deactivate();
		}

		public void Draw(Box2D rectangle, uint id)
		{
			var texCoords = SpriteSheet.CalcSpriteTexCoords(id);
			Texture.Activate();
			rectangle.DrawTexturedRect(texCoords);
			Texture.Deactivate();

		}

		public SpriteSheet SpriteSheet { get; private set; }
		public ITexture Texture { get; private set; }
	}
}
