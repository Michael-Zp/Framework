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
	class MyController : IWindow
	{
		private Level level = null;
		private Renderer renderer = new Renderer();

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var controller = new MyController();
			app.GameWindow.Resize += (s, e) => controller.renderer.Resize(app.GameWindow.Width, app.GameWindow.Height);
			app.Run(controller);
		}

		private MyController()
		{
			LoadLevel(LevelData.level1);

			GL.MatrixMode(MatrixMode.Projection);
			var size = Math.Max(level.Bounds.SizeX, level.Bounds.SizeY);
			GL.Ortho(0, size * 1, 0, size, 0, 1);
			GL.MatrixMode(MatrixMode.Modelview);
			//for transparency in textures we use blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);
		}

		private void LoadLevel(byte[] levelData)
		{
			using (var stream = new MemoryStream(levelData))
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