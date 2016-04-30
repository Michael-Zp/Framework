using OpenTK.Graphics.OpenGL;
using System;

namespace Framework
{
	public class TextureFont : IDisposable
	{
		public TextureFont(Texture texture, uint charactersPerLine = 16, byte firstAsciiCode = 0
			, float characterBoundingBoxWidth = 1.0f, float characterBoundingBoxHeight = 1.0f, float letterSpacing = 1.0f)
		{
			this.texFont = new SpriteSheet(texture, charactersPerLine, characterBoundingBoxWidth, characterBoundingBoxHeight);
			// Creating 256 Display Lists
			this.baseList = (uint)GL.GenLists(256);
			//foreach of the 256 possible characters create a a quad 
			//with texture coordinates and store it in a display list
			var rect = new AABR(0, 0, 1, 1);
			for (uint asciiCode = 0; asciiCode < 256; ++asciiCode)
			{
				GL.NewList((this.baseList + asciiCode), ListMode.Compile);
				texFont.Draw(asciiCode - firstAsciiCode, rect);
				GL.Translate(letterSpacing, 0, 0);	// Move To The next character
				GL.EndList();
			}
		}

		public void Dispose()
		{
			GL.DeleteLists(this.baseList, 256);	// Delete All 256 Display Lists
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
			GL.PushMatrix();
			GL.Translate(x, y, z);
			GL.Scale(size, size, size);
			var bytes = ConvertString2Ascii(text);
			texFont.BeginUse();
			PrintRawQuads(bytes);
			texFont.EndUse();
			GL.PopMatrix();
		}

		public float Width(string text, float size)
		{
			return text.Length * size;
		}

		private readonly uint baseList = 0;	// Base Display List For The Font
		private readonly SpriteSheet texFont;

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
