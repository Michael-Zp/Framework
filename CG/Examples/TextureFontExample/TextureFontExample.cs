﻿using Zenseless.Application;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class MyVisual
	{
		private TextureFont font;

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual();
			window.Render += visual.Render;
			window.Run();
		}

		private MyVisual()
		{
			//load font
			font = new TextureFont(TextureLoader.FromBitmap(Resourcen.Blood_Bath_2), 10, 32, .8f, 1, .7f);
			//background clear color
			GL.ClearColor(Color.Black);
			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend); // for transparency in textures
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
			GL.Color3(Color.White); //color is multiplied with texture color white == no change
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			font.Print(-.9f, -.125f, 0, .25f, "SUPER GEIL"); //print string
		}
	}
}