using Geometry;
using OpenTK.Graphics.OpenGL;

namespace Framework
{
	public static class DrawTools
	{
		public static void DrawTexturedRect(this Box2D rect, Box2D texCoords)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(texCoords.X, texCoords.Y); GL.Vertex2(rect.X, rect.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.Y); GL.Vertex2(rect.MaxX, rect.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.MaxY); GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.TexCoord2(texCoords.X, texCoords.MaxY); GL.Vertex2(rect.X, rect.MaxY);
			GL.End();
		}
	}
}
