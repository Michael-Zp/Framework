using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Example
{
	class MyWindow : IWindow
	{
		private PostProcessing postProcessing;
		private Stopwatch globalTime = new Stopwatch();
		private Box2D bird = Box2dExtensions.CreateFromCenterSize(0.0f, -0.8f, 0.3f, 0.3f);
		private Texture texBird;
		private Box2D background = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private Texture texBackground;

		private MyWindow(int width, int height)
		{
			texBackground = TextureLoader.FromBitmap(Resources.background);
			texBird = TextureLoader.FromBitmap(Resources.bird);

			postProcessing = new PostProcessing(width, height);
			try
			{
				postProcessing.SetShader(Encoding.UTF8.GetString(Resources.EdgeDetect));
			}
			catch (ShaderException e)
			{
				Console.WriteLine(e.ShaderLog);
			}

			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);

			globalTime.Start();
		}

		public void Render()
		{
			bool doPostProcessing = !Keyboard.GetState()[Key.Space];

			if (doPostProcessing) postProcessing.Start();

			GL.Color3(Color.White);
			//draw background
			texBackground.Activate();
			background.DrawTexturedRect(Box2D.BOX01);
			texBackground.Deactivate();

			//draw player
			texBird.Activate();
			bird.DrawTexturedRect(Box2D.BOX01);
			texBird.Deactivate();

			if (doPostProcessing) postProcessing.EndAndApply((float)globalTime.Elapsed.TotalSeconds);
		}

		public void Update(float updatePeriod)
		{
			var R = Transform2D.CreateRotationAroundOrigin(2.0f * updatePeriod);
			bird.TransformCenter(R);
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow(app.GameWindow.Width, app.GameWindow.Height));
		}
	}
}