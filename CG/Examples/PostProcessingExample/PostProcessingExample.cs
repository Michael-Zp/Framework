﻿using Zenseless.Application;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Text;
using Zenseless.Base;

namespace Example
{
	class MyVisual
	{
		private PostProcessing postProcessing;
		private Box2D bird = Box2DExtensions.CreateFromCenterSize(0.0f, -0.8f, 0.3f, 0.3f);
		private ITexture texBird;
		private IImmutableBox2D background = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private ITexture texBackground;

		private MyVisual(int width, int height)
		{
			texBackground = TextureLoader.FromBitmap(Resources.background);
			texBird = TextureLoader.FromBitmap(Resources.bird);

			postProcessing = new PostProcessing(width, height);
			try
			{
				postProcessing.SetShader(Encoding.UTF8.GetString(Resources.Swirl));
			}
			catch (ShaderException e)
			{
				Console.WriteLine(e.ShaderLog);
			}

			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		private void Render(float time)
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

			if (doPostProcessing) postProcessing.EndAndApply(time);
		}

		private void Update(float updatePeriod)
		{
			var t = new Transformation2D();
			t.RotateLocal(-200f * updatePeriod);
			bird.TransformCenter(t);
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual(window.GameWindow.Width, window.GameWindow.Height);
			var time = new GameTime();
			window.Render += () => visual.Render(time.Seconds);
			window.Update += visual.Update;
			window.Run();
		}
	}
}