using DMS.OpenGL;
using DMS.System;
using LevelData;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.IO;

namespace Example
{
	//todo: add logic; read logic data from level and render data; distribute
	class MyWindow : IWindow
	{
		private Level level = null;
		private Renderer renderer = new Renderer();

		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			var aspect = app.GameWindow.Width / (float)app.GameWindow.Height;
			app.Run(new MyWindow(aspect));
		}

		private MyWindow(float aspect)
		{
			LoadLevel();

			GL.MatrixMode(MatrixMode.Projection);
			var size = Math.Max(level.Bounds.SizeX, level.Bounds.SizeY);
			GL.Ortho(0, size * aspect, 0, size, 0, 1);
			GL.MatrixMode(MatrixMode.Modelview);
			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);
		}

		private void LoadLevel()
		{
			using (var stream = new MemoryStream(LevelData.level1))
			{
				level = Serialize.ObjFromBinStream(stream) as Level;
				foreach (var layer in level.layers.Values)
				{
					foreach (var sprite in layer)
					{
						renderer.Register(sprite, sprite.TextureName);
					}
				}
			}
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			foreach (var layer in level.layers.Values)
			{
				foreach (var sprite in layer)
				{
					renderer.Draw(sprite, sprite.RenderBounds);
				}
			}
		}

		public void Update(float updatePeriod)
		{
			float delta = updatePeriod * 100;
			//player movement
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			float axisUpDown = Keyboard.GetState()[Key.Down] ? -1.0f : Keyboard.GetState()[Key.Up] ? 1.0f : 0.0f;
			level.MovePlayer(delta * axisLeftRight, delta * axisUpDown);
		}
	}
}