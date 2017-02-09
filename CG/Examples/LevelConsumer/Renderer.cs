using Framework;
using Geometry;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

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
				texture.FilterTrilinear();
				texture.WrapMode(TextureWrapMode.Clamp);
				textures.Add(textureName, texture);
			}
			references[sprite] = texture;
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
