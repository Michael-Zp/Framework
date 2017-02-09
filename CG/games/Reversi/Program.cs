using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Reversi
{
	class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			MyApplication app = new MyApplication();
			app.gameWindow.Run();
		}

		public MyApplication()
		{
			gameWindow.Closing += GameWindow_Closing;
			gameWindow.Resize += GameWindow_Resize;
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.MouseDown += GameWindow_MouseDown;
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => { gameWindow.SwapBuffers(); };
			GL.ClearColor(Color.DarkGreen);
			for (int x = 0; x < 8; ++x)
			{
				for (int y = 0; y < 8; ++y)
				{
					grid[x, y] = FieldType.EMPTY;
				}
			}
			grid[3, 3] = FieldType.BLACK;
			grid[3, 4] = FieldType.WHITE;
			grid[4, 3] = FieldType.WHITE;
			grid[4, 4] = FieldType.BLACK;
			lastMove = new Point(4, 4);
			texWhite = TextureLoader.FromBitmap(Resourcen.white);
			texBlack = TextureLoader.FromBitmap(Resourcen.black);
			texTable = TextureLoader.FromBitmap(Resourcen.pool_table);
		}

		private enum FieldType { EMPTY, BLACK, WHITE };
		private FieldType[,] grid = new FieldType[8, 8];
		private GameWindow gameWindow = new GameWindow(1024, 1024);
		private Matrix4 toClipSpace = new Matrix4();
		private bool whiteMoves = true;
		private TextureFont font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Fire_2), 10, 32, 1.0f, 0.9f, 0.5f);
		private Texture texWhite;
		private Texture texBlack;
		private Texture texTable;
		private Point lastMove;

		private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (DialogResult.Yes != MessageBox.Show("End game?", "Exit", MessageBoxButtons.YesNo))
			{
				e.Cancel = true;
			}
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key)
			{
				gameWindow.Exit();
			}
		}

		private void GameWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.Button != MouseButton.Left) return;
			//calculate the grid coordinates
			var pos = new Vector4(2.0f * e.X / (gameWindow.Width - 1.0f) - 1.0f, -2.0f * e.Y / (gameWindow.Height - 1.0f) + 1.0f, 0.0f, 1.0f);
			var fromClipSpace = toClipSpace.Inverted();
			var gridPos = Vector4.Transform(pos, fromClipSpace);
			//do the move
			Move((int)Math.Floor(gridPos.X), (int)Math.Floor(gridPos.Y));
		}

		private void Move(int x, int y)
		{
			if (x < 0 || 7 < x) return;
			if (y < 0 || 7 < y) return;
			if (FieldType.EMPTY != grid[x, y]) return;
			var color = whiteMoves ? FieldType.WHITE : FieldType.BLACK;
			grid[x, y] = color;
			lastMove = new Point(x, y); 
			for (int dirX = -1; dirX <= 1; ++dirX)
			{
				for (int dirY = -1; dirY <= 1; ++dirY)
				{
					if (0 == dirX && 0 == dirY) continue;
					Reverse(x, y, color, dirX, dirY);
				}
			}
			whiteMoves = !whiteMoves;
		}

		private void Reverse(int startX, int startY, FieldType fillColor, int dirX, int dirY)
		{
			var otherColor = FieldType.BLACK == fillColor ? FieldType.WHITE : FieldType.BLACK;
			//search how many to reverse
			for (int i = 1; true; ++i)
			{
				//go one step into direction
				int x = startX + i * dirX;
				int y = startY + i * dirY;
				//check out of bounds
				if (x < 0 || 7 < x) return;
				if (y < 0 || 7 < y) return;
				if (otherColor != grid[x, y])
				{
					if (fillColor == grid[x, y])
					{
						for (int j = 1; j < i; ++j)
						{
							//reverse
							int reverseX = startX + j * dirX;
							int reverseY = startY + j * dirY;
							grid[reverseX, reverseY] = fillColor;
						}
					}
					return;
				}
			}
		}

		private void GameWindow_Resize(object sender, EventArgs e)
		{
			var w = gameWindow.Width;
			var h = gameWindow.Height;
			var size = 8.0f;

			GL.Viewport(0, 0, w, h);
			GL.MatrixMode(MatrixMode.Projection);
			if (w >= h)
			{
				var emptyPart = (w / (float)h - 1.0f) * size * 0.5f;
				toClipSpace = Matrix4.CreateOrthographicOffCenter(-emptyPart, emptyPart + size, 0.0f, size, 0.0f, 1.0f);
			}
			else
			{
				var emptyPart = (h / (float)w - 1.0f) * size * 0.5f;
				toClipSpace = Matrix4.CreateOrthographicOffCenter(0.0f, size, -emptyPart, emptyPart + size, 0.0f, 1.0f);
			}
			GL.LoadMatrix(ref toClipSpace);
			GL.MatrixMode(MatrixMode.Modelview);
		}
		
		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawField();
			//which player moves?
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Color4(1.0, 1.0, 1.0, 1.0);
			GL.Disable(EnableCap.Blend);

			PrintWinLooseMessage();
		}

		private void PrintWinLooseMessage()
		{
			int countWhite = 0;
			int countBlack = 0;
			for (int x = 0; x < 8; ++x)
			{
				for (int y = 0; y < 8; ++y)
				{
					switch (grid[x, y])
					{
						case FieldType.BLACK: ++countBlack; break;
						case FieldType.WHITE: ++countWhite; break;
					}
				}
			}
			gameWindow.Title = "white:" + countWhite.ToString() + " black:" + countBlack.ToString();
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

		private void DrawField()
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
					var type = grid[x, y];
					if (FieldType.EMPTY == type) continue;
					DrawSprite(x + 0.5f, y + 0.5f, FieldType.BLACK == type ? texBlack : texWhite, 0.45f);
				}
			}
			GL.Disable(EnableCap.Blend);
			DrawSelection(lastMove.X, lastMove.Y);
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