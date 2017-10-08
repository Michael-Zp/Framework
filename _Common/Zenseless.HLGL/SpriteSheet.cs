using Zenseless.Geometry;

namespace Zenseless.HLGL
{
	/// <summary>
	/// class for grid based sprite sheets with equal sized rectangular sprites
	/// </summary>
	public class SpriteSheet
	{
		/// <summary>
		/// create a new isntance
		/// </summary>
		/// <param name="tex"></param>
		/// <param name="spritesPerRow"></param>
		/// <param name="spritesPerColumn"></param>
		/// <param name="spriteBoundingBoxWidth"></param>
		/// <param name="spriteBoundingBoxHeight"></param>
		public SpriteSheet(ITexture tex, uint spritesPerRow, uint spritesPerColumn
			, float spriteBoundingBoxWidth = 1.0f, float spriteBoundingBoxHeight = 1.0f)
		{
			Tex = tex;
			Tex.Filter = TextureFilterMode.Mipmap;
			SpritesPerRow = spritesPerRow;
			SpritesPerColumn = spritesPerColumn;
			SpriteBoundingBoxWidth = spriteBoundingBoxWidth;
			SpriteBoundingBoxHeight = spriteBoundingBoxHeight;
		}

		/// <summary>
		/// Calculates texture coordinates for a given sprite id uses CalcSpriteTexCoords
		/// </summary>
		/// <param name="spriteID">number of sprite 0-based; counted from left-top</param>
		/// <returns>texture coordinates of sprite</returns>
		public Box2D CalcSpriteTexCoords(uint spriteID)
		{
			return CalcSpriteTexCoords(spriteID, SpritesPerRow, SpritesPerColumn, SpriteBoundingBoxWidth, SpriteBoundingBoxHeight);
		}

		/// <summary>
		/// Calculates texture coordinates for a given sprite id
		/// </summary>
		/// <param name="spriteID">number of sprite 0-based; counted from left-top</param>
		/// <param name="spritesPerRow">number of sprites per row</param>
		/// <param name="spritesPerColumn">number of sprites per column</param>
		/// <param name="spriteBoundingBoxWidth"></param>
		/// <param name="spriteBoundingBoxHeight"></param>
		/// <returns>texture coordinates of sprite</returns>
		public static Box2D CalcSpriteTexCoords(uint spriteID, uint spritesPerRow, uint spritesPerColumn
			, float spriteBoundingBoxWidth = 1.0f, float spriteBoundingBoxHeight = 1.0f)
		{
			uint row = spriteID / spritesPerRow;
			uint col = spriteID % spritesPerRow;

			float centerX = (col + 0.5f) / spritesPerRow;
			float centerY = 1.0f - (row + 0.5f) / spritesPerColumn;
			float width = spriteBoundingBoxWidth / spritesPerRow;
			float height = spriteBoundingBoxHeight / spritesPerColumn;
			
			return new Box2D(centerX - 0.5f * width, centerY - 0.5f * height, width, height);
		}

		public void Activate()
		{
			Tex.Activate();
		}

		public void Deactivate()
		{
			Tex.Deactivate();
		}

		public float SpriteBoundingBoxWidth { get; private set; }

		public float SpriteBoundingBoxHeight { get; private set; }

		public uint SpritesPerRow { get; private set; }

		public uint SpritesPerColumn { get; private set; }

		public ITexture Tex { get; private set; }
	}
}
