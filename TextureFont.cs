using OpenTK.Graphics.OpenGL;
using System;

namespace Framework
{
	public class TextureFont : IDisposable
	{
		public TextureFont(Texture texture, uint charactersPerLine = 16, byte firstAsciiCode = 0
			, float characterBoundingBoxWidth = 1.0f, float characterBoundingBoxHeight = 1.0f, float letterSpacing = 1.0f)
		{
			this.texFont = texture;
			this.texFont.FilterTrilinear();
			// Creating 256 Display Lists
			this.baseList = (uint)GL.GenLists(256);					
			//foreach of the 256 possible characters create a a quad 
			//with texture coordinates and store it in a display list
			for (uint asciiCode = 0; asciiCode < 256; ++asciiCode)
			{
				GL.NewList((this.baseList + asciiCode), ListMode.Compile);
				DrawSpriteQuad(asciiCode - firstAsciiCode, charactersPerLine, characterBoundingBoxWidth, characterBoundingBoxHeight);
				GL.Translate(letterSpacing, 0, 0);	// Move To The next character
				GL.EndList();
			}
		}

		public void Dispose()
		{
			GL.DeleteLists(this.baseList, 256);	// Delete All 256 Display Lists
		}

		public static AABR CalcSpriteTexCoords(uint spriteID, uint spritesPerLine
			, float spriteBoundingBoxWidth = 1.0f, float spriteBoundingBoxHeight = 1.0f)
		{
			uint row = spriteID / spritesPerLine;
			uint col = spriteID % spritesPerLine;

			float centerX = (col + 0.5f) / spritesPerLine;
			float centerY = 1.0f - (row + 0.5f) / spritesPerLine;
			float height = spriteBoundingBoxHeight / spritesPerLine;
			float width = spriteBoundingBoxWidth / spritesPerLine;
			return new AABR(centerX - 0.5f * width, centerY - 0.5f * height, width, height);
		}

		public byte[] ConvertString2Ascii(string text)
		{
			byte[] bytes = new byte[text.Length];
			uint pos = 0;
			foreach (char c in text)
			{
				bytes[pos] = (byte)c;
				++pos;
			}
			return bytes;
		}

		public void Print(float x, float y, float z, float size, string text)
		{
			texFont.BeginUse();
			GL.PushMatrix();
			GL.Translate(x, y, z);
			GL.Scale(size, size, size);
			var bytes = ConvertString2Ascii(text);
			PrintRawQuads(bytes);
			GL.PopMatrix();
			texFont.EndUse();
		}

		public float Width(string text, float size)
		{
			return text.Length * size;
		}

		private readonly uint baseList = 0;	// Base Display List For The Font
		private readonly Texture texFont;

		private static void DrawSpriteQuad(uint spriteID, uint spritesPerLine
			, float spriteBoundingBoxWidth, float spriteBoundingBoxHeight)
		{
			AABR rect = CalcSpriteTexCoords(spriteID, spritesPerLine, spriteBoundingBoxWidth, spriteBoundingBoxHeight);

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(rect.X, rect.Y); GL.Vertex2(0, 0);
			GL.TexCoord2(rect.X, rect.MaxY); GL.Vertex2(0, 1);
			GL.TexCoord2(rect.MaxX, rect.MaxY); GL.Vertex2(1, 1);
			GL.TexCoord2(rect.MaxX, rect.Y); GL.Vertex2(1, 0);
			GL.End();
		}

		private void PrintRawQuads(byte[] text)
		{
			if (null == text) return;
			GL.PushAttrib(AttribMask.ListBit);
			GL.PushMatrix();
			GL.ListBase(this.baseList);
			GL.CallLists(text.Length, ListNameType.UnsignedByte, text); // Write The Text To The Screen
			GL.PopMatrix();
			GL.PopAttrib();
		}
	}
}
