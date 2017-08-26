using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using DMS.Application;

namespace Pong
{
	class Game
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var game = new Game();
			window.Update += game.Update;
			window.Render += game.Render;
			window.Run();
		}

		private Game()
		{
			ResetBall(true);
			GL.ClearColor(Color.Black);
		}

		private TextureFont font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Big_Cheese), 10, 32);
		private Box2D paddle1 = new Box2D(-0.95f, -0.2f, 0.05f, 0.4f);
		private Box2D paddle2 = new Box2D(0.9f, -0.2f, 0.05f, 0.4f);
		private Box2D ball = new Box2D(0.0f, 0.0f, 0.1f, 0.1f);
		private Vector2 ballV = new Vector2(1.0f, 0.0f);
		private int player1Points = 0;
		private int player2Points = 0;

		private void ResetBall(bool toPlayer2)
		{
			ball.MinX = 0.0f;
			ball.MinY = 0.0f;
			ballV = new Vector2(toPlayer2 ? 1.0f : -1.0f, 0.0f);
		}

		private static float MovePaddle(float paddleY, float updatePeriod, bool up, bool down)
		{
			if (down)
			{
				paddleY -= updatePeriod;
			}
			if (up)
			{
				paddleY += updatePeriod;
			}
			return OpenTK.MathHelper.Clamp(paddleY, -1.0f, 0.6f);
		}

		private static float PaddleBallResponse(Box2D paddle, Box2D ball)
		{
			float vY = (paddle.CenterY - ball.CenterY) / (0.5f * paddle.SizeY);
			vY = OpenTK.MathHelper.Clamp(vY, -2.0f, 2.0f);
			return vY;
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawPaddle(paddle1);
			DrawPaddle(paddle2);
			DrawCircle(ball.CenterX, ball.CenterY, 0.5f * ball.SizeX);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
			GL.Color4(1.0, 1.0, 1.0, 1.0);
			string score = player1Points.ToString() + '-' + player2Points.ToString();
			font.Print(-0.5f * font.Width(score, 0.1f), -0.9f, 0.0f, 0.1f, score);
			GL.Disable(EnableCap.Blend);
		}

		private void Update(float updatePeriod)
		{
			if (Keyboard.GetState()[Key.Space])
			{
				ResetBall(true);
			}

			paddle1.MinY = MovePaddle(paddle1.MinY, updatePeriod, Keyboard.GetState()[Key.Q], Keyboard.GetState()[Key.A]);
			paddle2.MinY = MovePaddle(paddle2.MinY, updatePeriod, Keyboard.GetState()[Key.O], Keyboard.GetState()[Key.L]);
			//move ball
			ball.MinX += updatePeriod * ballV.X;
			ball.MinY += updatePeriod * ballV.Y;
			//reflect ball
			if (ball.MaxY > 1.0f || ball.MinY < -1.0)
			{
				ballV.Y = -ballV.Y;
			}
			//points
			if (ball.MinX > 1.0f) 
			{
				++player1Points;
				ResetBall(false);
			}
			if (ball.MaxX < -1.0)
			{
				++player2Points;
				ResetBall(true);
			}
			//paddle vs ball
			if (paddle1.Intersects(ball))
			{
				ballV.Y = PaddleBallResponse(paddle1, ball);
				ballV.X = 1.0f;
			}
			if (paddle2.Intersects(ball))
			{
				ballV.Y = PaddleBallResponse(paddle2, ball);
				ballV.X = -1.0f;
			}
		}

		static void DrawCircle(float centerX, float centerY, float radius)
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

		static void DrawPaddle(Box2D frame)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(Color.Green);
			GL.Vertex2(frame.MinX, frame.MinY);
			GL.Vertex2(frame.MaxX, frame.MinY);
			GL.Vertex2(frame.MaxX, frame.MaxY);
			GL.Vertex2(frame.MinX, frame.MaxY);
			GL.End();
		}
	}
}