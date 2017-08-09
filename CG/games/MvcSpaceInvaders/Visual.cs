using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using DMS.HLGL;

namespace MvcSpaceInvaders
{
	public class Visual
	{
		public Visual()
		{
			texPlayer = TextureLoader.FromBitmap(Resourcen.blueships1);
			texEnemy = TextureLoader.FromBitmap(Resourcen.redship4);
			texBullet = TextureLoader.FromBitmap(Resourcen.blueLaserRay);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		public void DrawScreen(IEnumerable<Box2D> enemies, IEnumerable<Box2D> bullets, Box2D player)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();

			texEnemy.Activate();
			foreach (Box2D enemy in enemies)
			{
				Draw(enemy);
			}
			texEnemy.Deactivate();
			texBullet.Activate();
			foreach (Box2D bullet in bullets)
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

		private void Draw(Box2D Rectanlge)
		{
			GL.Color3(Color.White);
			GL.Begin(PrimitiveType.Quads);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rectanlge.MinX, Rectanlge.MinY);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rectanlge.MaxX, Rectanlge.MinY);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rectanlge.MaxX, Rectanlge.MaxY);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rectanlge.MinX, Rectanlge.MaxY);
			GL.End();
		}
	}
}
