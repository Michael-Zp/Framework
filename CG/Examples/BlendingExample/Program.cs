using Framework;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			//setup
			gameWindow = new GameWindow(700, 700);
			gameWindow.VSync = VSyncMode.On;
			gameWindow.KeyDown += (s, arg) => gameWindow.Close();
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += GameWindow_RenderFrame;			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			//background clear color
			GL.ClearColor(1, 1, 1, 1);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//setup blending equation Color = Color_s · alpha + Color_d · (1 - alpha)
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BlendEquation(BlendEquationMode.FuncAdd);


			GL.Enable(EnableCap.Blend);
			var rect = new Box2D(-.75f, -.75f, .5f, .5f);
			DrawRect(rect, new Color4(.5f, .7f, .1f, 1));
			rect.X += .25f;
			rect.Y += .25f;
			DrawRect(rect, new Color4(.7f, .5f, .9f, .5f));
			rect.X += .25f;
			rect.Y += .25f;
			DrawRect(rect, new Color4(.7f, .5f, .9f, .5f));
			rect.X += .25f;
			rect.Y += .25f;
			DrawRect(rect, new Color4(.5f, .7f, 1, .5f));
			rect.X += .25f;
			rect.Y += .25f;
			DrawRect(rect, new Color4(.5f, .7f, 1, .5f));
			GL.Disable(EnableCap.Blend);
		}

		private void DrawRect(Box2D rectangle, Color4 color)
		{
			GL.Color4(color);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(rectangle.X, rectangle.Y);
			GL.Vertex2(rectangle.MaxX, rectangle.Y);
			GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.Vertex2(rectangle.X, rectangle.MaxY);
			GL.End();
		}
	}
}