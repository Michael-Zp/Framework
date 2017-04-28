using DMS.OpenGL;
using DMS.Geometry;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;

namespace Example
{
	public class Renderer
	{
		public void Register(object sprite, string textureName)
		{
			Texture texture;
			if(!textures.TryGetValue(textureName, out texture))
			{
				texture = TextureLoader.FromFile(@"..\..\LevelEditor\" + textureName);
				texture.FilterMipmap();
				texture.WrapMode(TextureWrapMode.Clamp);
				textures.Add(textureName, texture);
			}
			references[sprite] = texture;
		}

		public void Resize(int width, int height)
		{
			var aspect = width / (float)height;
			//GL.MatrixMode(MatrixMode.Projection);
			//var size = Math.Max(level.Bounds.SizeX, level.Bounds.SizeY);
			//GL.Ortho(0, size * aspect, 0, size, 0, 1);
			//GL.MatrixMode(MatrixMode.Modelview);
		}

		public void Draw(object reference, Box2D bounds)
		{
			references[reference].Activate();
			bounds.DrawTexturedRect(Box2D.BOX01);
			references[reference].Deactivate();
		}

		private Dictionary<object, Texture> references = new Dictionary<object, Texture>();
		private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
	}
}
