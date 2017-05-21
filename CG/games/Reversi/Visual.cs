using DMS.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;

namespace Reversi
{
	public class Visual
	{
		public Visual()
		{
			GL.ClearColor(Color.DarkGreen);
			texWhite = TextureLoader.FromBitmap(Resourcen.white);
			texBlack = TextureLoader.FromBitmap(Resourcen.black);
			texTable = TextureLoader.FromBitmap(Resourcen.pool_table);
		}

		public void Resize(int width, int height)
		{
			var size = 8.0f;

			GL.Viewport(0, 0, width, height);
			GL.MatrixMode(MatrixMode.Projection);
			if (width >= height)
			{
				var emptyPart = (width / (float)height - 1.0f) * size * 0.5f;
				toClipSpace = Matrix4.CreateOrthographicOffCenter(-emptyPart, emptyPart + size, 0.0f, size, 0.0f, 1.0f);
			}
			else
			{
				var emptyPart = (height / (float)width - 1.0f) * size * 0.5f;
				toClipSpace = Matrix4.CreateOrthographicOffCenter(0.0f, size, -emptyPart, emptyPart + size, 0.0f, 1.0f);
			}
			GL.LoadMatrix(ref toClipSpace);
			GL.MatrixMode(MatrixMode.Modelview);
		}

		public Point CalcGridPosFromNormalized(Vector2 coord)
		{
			//calculate the grid coordinates
			var pos = new Vector4(2.0f * coord.X - 1.0f, 2.0f * coord.Y - 1.0f, 0.0f, 1.0f);
			var fromClipSpace = toClipSpace.Inverted();
			var gridPos = Vector4.Transform(pos, fromClipSpace);
			return new Point((int)Math.Floor(gridPos.X), (int)Math.Floor(gridPos.Y));
		}

		public void Render(IGameState gameState)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawField(gameState);
			//which player moves?
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Color4(1.0, 1.0, 1.0, 1.0);
			GL.Disable(EnableCap.Blend);

			PrintWinLooseMessage(gameState);
		}

		private Matrix4 toClipSpace = new Matrix4();
		private TextureFont font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Fire_2), 10, 32, 1.0f, 0.9f, 0.5f);
		private Texture texWhite;
		private Texture texBlack;
		private Texture texTable;

		private void PrintWinLooseMessage(IGameState gameState)
		{
			int countWhite = 0;
			int countBlack = 0;
			for (int x = 0; x < 8; ++x)
			{
				for (int y = 0; y < 8; ++y)
				{
					switch (gameState[x, y])
					{
						case FieldType.BLACK: ++countBlack; break;
						case FieldType.WHITE: ++countWhite; break;
					}
				}
			}
			//gameWindow.Title = "white:" + countWhite.ToString() + " black:" + countBlack.ToString();
			if (8 * 8 == countWhite + countBlack)
			{
				//win/loose
				GL.Enable(EnableCap.Blend);
				GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				GL.Color4(1.0, 1.0, 1.0, 1.0);
				string score = countWhite.ToString() + '-' + countBlack.ToString();
				font.Print(4.0f - 0.5f * font.Width(score, 1.0f), 4.0f, 0.0f, 1.0f, score);
				string winner = countWhite > countBlack ? "White" : "Black";
				winner = countWhite == countBlack ? "Draw" : winner + " wins";
				font.Print(4.0f - 0.5f * font.Width(score, 1.0f), 3.0f, 0.0f, 1.0f, winner);
				GL.Disable(EnableCap.Blend);
			}
		}

		private void DrawField(IGameState gameState)
		{
			DrawSprite(4.0f, 4.0f, texTable, 4.0f, 8.0f);
			//grid
			GL.Color3(Color.Black);
			GL.LineWidth(3.0f);
			GL.Begin(PrimitiveType.Lines);
			for (int i = 0; i < 9; ++i)
			{
				GL.Vertex2(i, 0.0);
				GL.Vertex2(i, 8.0);
			}
			for (int i = 0; i < 9; ++i)
			{
				GL.Vertex2(0.0, i);
				GL.Vertex2(8.0, i);
			}
			GL.End();
			//chips
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			for (int x = 0; x < 8; ++x)
			{
				for (int y = 0; y < 8; ++y)
				{
					var type = gameState[x, y];
					if (FieldType.EMPTY == type) continue;
					DrawSprite(x + 0.5f, y + 0.5f, FieldType.BLACK == type ? texBlack : texWhite, 0.45f);
				}
			}
			GL.Disable(EnableCap.Blend);
			DrawSelection(gameState.LastMoveX, gameState.LastMoveY);
		}

		static void DrawSelection(int x_, int y_)
		{
			float x = x_ + 0.5f;
			float y = y_ + 0.5f;
			float radius = 0.48f;
			GL.Color3(Color.Blue);
			GL.LineWidth(4.0f);
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(x - radius, y - radius);
			GL.Vertex2(x + radius, y - radius);
			GL.Vertex2(x + radius, y + radius);
			GL.Vertex2(x - radius, y + radius);
			GL.End();
		}

		static void DrawSprite(float x, float y, Texture tex, float radius = 0.5f, float repeat = 1.0f)
		{
			tex.Activate();
			GL.Color3(Color.White);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(x - radius, y - radius);
			GL.TexCoord2(repeat, 0.0f); GL.Vertex2(x + radius, y - radius);
			GL.TexCoord2(repeat, repeat); GL.Vertex2(x + radius, y + radius);
			GL.TexCoord2(0.0f, repeat); GL.Vertex2(x - radius, y + radius);
			GL.End();
			tex.Deactivate();
		}
	}
}
