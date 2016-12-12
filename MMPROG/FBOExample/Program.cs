using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using System.Text;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow(1024, 1024);
		private FBO fbo;
		private Texture textureForRendering;
		private Shader shaderPostProcess;
		private Shader shaderSource;
		private Stopwatch globalTime = new Stopwatch();

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			gameWindow.KeyDown += GameWindow_KeyDown;

			try
			{
				fbo = new FBO();
				textureForRendering = Texture.Create(gameWindow.Width, gameWindow.Height);
				shaderPostProcess = PostProcessingShader.Create(Encoding.UTF8.GetString(Resources.ChromaticAberration));
				shaderSource = PostProcessingShader.Create(Encoding.UTF8.GetString(Resources.PatternCircle));
			}
			catch (ShaderException e)
			{
				Console.WriteLine(e.Log);
			}

			globalTime.Start();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key)
			{
				gameWindow.Exit();
			}
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			bool doPostProcessing = !Keyboard.GetState()[Key.Space];
			float time = (float)globalTime.Elapsed.TotalSeconds;
			int width = gameWindow.Width;
			int height = gameWindow.Height;

			if (doPostProcessing)
			{
				fbo.BeginUse(textureForRendering); //start drawing into texture
				GL.Viewport(0, 0, textureForRendering.Width, textureForRendering.Height);
			}

			//draw staff
			shaderSource.Begin();
			GL.Uniform2(shaderSource.GetUniformLocation("iResolution"), (float)width, (float)height);
			GL.Uniform1(shaderSource.GetUniformLocation("iGlobalTime"), time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderSource.End();

			if (doPostProcessing)
			{
				fbo.EndUse(); //stop drawing into texture
				GL.Viewport(0, 0, width, height);
				textureForRendering.BeginUse();
				shaderPostProcess.Begin();
				GL.Uniform2(shaderPostProcess.GetUniformLocation("iResolution"), (float)width, (float)height);
				GL.Uniform1(shaderPostProcess.GetUniformLocation("iGlobalTime"), time);
				GL.DrawArrays(PrimitiveType.Quads, 0, 4);
				shaderPostProcess.End();
				textureForRendering.EndUse();
			}
		}
	}
}