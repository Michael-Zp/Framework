using Framework;
using Geometry;
using LevelEditor;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();
		private Level level = null;
		private Renderer renderer = new Renderer();

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			//run the update loop, which calls our registered callbacks
			app.gameWindow.Run();
		}

		private MyApplication()
		{
			LoadLevel();
			gameWindow.WindowState = WindowState.Fullscreen;
			GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			gameWindow.KeyDown += GameWindow_KeyDown;
			GL.MatrixMode(MatrixMode.Projection);
			GL.Ortho(0, 1, 0, 1, 0, 1);
			GL.MatrixMode(MatrixMode.Modelview);
			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);
		}

		private void LoadLevel()
		{
			level = Serialize.ObjFromBinFile(@"..\..\LevelEditor\level.data") as Level;
			foreach(var layer in level.layers.Values)
			{
				foreach(var sprite in layer)
				{
					renderer.Register(sprite, sprite.TextureName);
				}
			}
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key)
			{
				gameWindow.Exit();
			}
		}

		private void GameWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			float delta = (float)gameWindow.UpdatePeriod * 0.3f;
			//player movement
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			float axisUpDown = Keyboard.GetState()[Key.Down] ? -1.0f : Keyboard.GetState()[Key.Up] ? 1.0f : 0.0f;
			level.MovePlayer(delta * axisLeftRight, delta * axisUpDown);
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			foreach (var layer in level.layers.Values)
			{
				foreach (var sprite in layer)
				{
					renderer.Draw(sprite, sprite.Bounds);
				}
			}
		}
	}
}