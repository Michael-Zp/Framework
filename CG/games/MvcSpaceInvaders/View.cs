using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using Zenseless.HLGL;

namespace MvcSpaceInvaders
{
	public class View
	{
		public View()
		{
			texPlayer = TextureLoader.FromBitmap(Resourcen.blueships1);
			texEnemy = TextureLoader.FromBitmap(Resourcen.redship4);
			texBullet = TextureLoader.FromBitmap(Resourcen.blueLaserRay);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		public void DrawScreen(IEnumerable<IReadOnlyBox2D> enemies, IEnumerable<IReadOnlyBox2D> bullets, IReadOnlyBox2D player)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Color3(Color.White);

			texEnemy.Activate();
			foreach (var enemy in enemies)
			{
				Draw(enemy);
			}
			texEnemy.Deactivate();
			texBullet.Activate();
			foreach (var bullet in bullets)
			{
				Draw(bullet);
			}
			texBullet.Deactivate();
			texPlayer.Activate();
			Draw(player);
			texPlayer.Deactivate();
		}

		private readonly ITexture texEnemy;
		private readonly ITexture texPlayer;
		private readonly ITexture texBullet;

		private void Draw(IReadOnlyBox2D rectangle)
		{
			GL.Begin(PrimitiveType.Quads);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(rectangle.MinX, rectangle.MinY);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(rectangle.MaxX, rectangle.MinY);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(rectangle.MinX, rectangle.MaxY);
			GL.End();
		}
	}
}
