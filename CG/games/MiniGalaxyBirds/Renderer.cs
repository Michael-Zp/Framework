using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Zenseless.HLGL;

namespace MiniGalaxyBirds
{
	public class Renderer : IRenderer
	{
		public Renderer()
		{
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		public void Register(string type, ITexture texture)
		{
			this.registeredTypes[type] = texture;
		}

		public void RegisterFont(TextureFont textureFont)
		{
			this.font = textureFont;
		}

		public void ResizeWindow(int width, int height)
		{
			float deltaX = 0.5f * ((width / (float)height) - 1.0f);
			GL.Viewport(0, 0, width, height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-deltaX, 1.0 + deltaX, 0.0, 1.0, 0.0, 1.0);
			GL.MatrixMode(MatrixMode.Modelview);
		}

		public IDrawable CreateDrawable(string type, Box2D frame)
		{
			if (this.registeredTypes.TryGetValue(type, out ITexture tex))
			{
				IDrawable drawable = new Sprite(tex, frame);
				drawables.Add(drawable);
				return drawable;
			}
			throw new Exception("Unregisterd type " + type.ToString());
		}

		public IDrawable CreateDrawable(string type, Box2D frame, IAnimation animation)
		{
			if (this.registeredTypes.TryGetValue(type, out ITexture tex))
			{
				IDrawable drawable = new AnimatedSprite(tex, frame, animation);
				drawables.Add(drawable);
				return drawable;
			}
			throw new Exception("Unregisterd type " + type.ToString());
		}

		public void DeleteDrawable(IDrawable drawable)
		{
			drawables.Remove(drawable);
		}

		public void DrawScreen(Box2D clipFrame, uint points)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			if (!ReferenceEquals(null,  clipFrame))
			{
				foreach (IDrawable drawable in drawables)
				{
					if (clipFrame.Intersects(drawable.Rect))
					{
						drawable.Draw();
					}
				}
			}
			else
			{
				foreach (IDrawable drawable in drawables)
				{
					drawable.Draw();
				}
			}
			Print(-0.15f, 0.0f, 0.0f, 0.04f, points.ToString());
		}

		public void Print(float x, float y, float z, float size, string text)
		{
			if (ReferenceEquals(null,  font))
			{
				throw new Exception("No font registered!");
			}
			font.Print(x, y, z, size, text);
		}

		private readonly Dictionary<string, ITexture> registeredTypes = new Dictionary<string, ITexture>();
		private readonly HashSet<IDrawable> drawables = new HashSet<IDrawable>();

		private TextureFont font = null;
	}
}
