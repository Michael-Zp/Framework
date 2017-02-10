using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Pong
{
	class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			MyApplication app = new MyApplication();
			app.gameWindow.Run(60.0);
		}

		public MyApplication()
		{
			gameWindow.KeyUp += (sender, e) => { if (e.Key == Key.Space) { resetBall(true); } };
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => { gameWindow.SwapBuffers(); };
			resetBall(true);
			GL.ClearColor(Color.Black);
		}

		private GameWindow gameWindow = new GameWindow(1024, 1024);
		private TextureFont font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Big_Cheese), 10, 32);
		private Box2D paddle1 = new Box2D(-0.95f, -0.2f, 0.05f, 0.4f);
		private Box2D paddle2 = new Box2D(0.9f, -0.2f, 0.05f, 0.4f);
		private Box2D ball = new Box2D(0.0f, 0.0f, 0.1f, 0.1f);
		private Vector2 ballV = new Vector2(1.0f, 0.0f);
		private int player1Points = 0;
		private int player2Points = 0;
		
		private void resetBall(bool toPlayer2)
		{
			ball.X = 0.0f;
			ball.Y = 0.0f;
			ballV = new Vector2(toPlayer2 ? 1.0f : -1.0f, 0.0f);
		}

		private static float movePaddle(float paddleY, bool up, bool down)
		{
			if (down)
			{
				paddleY -= 0.03f;
			}
			if (up)
			{
				paddleY += 0.03f;
			}
			return OpenTK.MathHelper.Clamp(paddleY, -1.0f, 0.6f);
		}

		private static float paddleBallResponse(Box2D paddle, Box2D ball)
		{
			float vY = (paddle.CenterY - ball.CenterY) / (0.5f * paddle.SizeY);
			vY = OpenTK.MathHelper.Clamp(vY, -2.0f, 2.0f);
			Console.WriteLine(vY);
			return vY;
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (Keyboard.GetState()[Key.Escape])
			{
				gameWindow.Exit();
			}
			paddle1.Y = movePaddle(paddle1.Y, Keyboard.GetState()[Key.Q], Keyboard.GetState()[Key.A]);
			paddle2.Y = movePaddle(paddle2.Y, Keyboard.GetState()[Key.O], Keyboard.GetState()[Key.L]);
			//move ball
			ball.X += 1.0f / 60.0f * ballV.X;
			ball.Y += 1.0f / 60.0f * ballV.Y;
			//reflect ball
			if (ball.MaxY > 1.0f || ball.Y < -1.0)
			{
				ballV.Y = -ballV.Y;
			}
			//points
			if (ball.X > 1.0f) 
			{
				++player1Points;
				resetBall(false);
			}
			if (ball.MaxX < -1.0)
			{
				++player2Points;
				resetBall(true);
			}
			//paddle vs ball
			if (paddle1.Intersects(ball))
			{
				ballV.Y = paddleBallResponse(paddle1, ball);
				ballV.X = 1.0f;
			}
			if (paddle2.Intersects(ball))
			{
				ballV.Y = paddleBallResponse(paddle2, ball);
				ballV.X = -1.0f;
			}
		}

		static void drawCircle(float centerX, float centerY, float radius)
		{
			GL.Begin(PrimitiveType.Polygon);
			GL.Color3(Color.Red);
			for (float alpha = 0.0f; alpha < 2 * Math.PI; alpha += 0.1f * (float)Math.PI)
			{
				float x = radius * (float)Math.Cos(alpha);
				float y = radius * (float)Math.Sin(alpha);
				GL.Vertex2(centerX + x, centerY + y);
			}
			GL.End();
		}

		static void drawPaddle(Box2D frame)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(Color.Green);
			GL.Vertex2(frame.X, frame.Y);
			GL.Vertex2(frame.MaxX, frame.Y);
			GL.Vertex2(frame.MaxX, frame.MaxY);
			GL.Vertex2(frame.X, frame.MaxY);
			GL.End();
		}

		void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			drawPaddle(paddle1);
			drawPaddle(paddle2);
			drawCircle(ball.CenterX, ball.CenterY, 0.5f * ball.SizeX);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Color4(1.0, 1.0, 1.0, 1.0);
			string score = player1Points.ToString() + '-' + player2Points.ToString();
			font.Print(-0.5f * font.Width(score, 0.1f), -0.9f, 0.0f, 0.1f, score);
			GL.Disable(EnableCap.Blend);
		}
	}
}