using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Zenseless.Application
{
	public class ExampleWindow
	{
		public ExampleWindow(int width = 512, int height = 512, double updateRate = 60)
		{
			//var mode = new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24);
			//gameWindow = new GameWindow(width, height, mode, "", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, OpenTK.Graphics.GraphicsContextFlags.Default);
			gameWindow = new GameWindow(width, height);
			RenderContext = new RenderContextGL();

			var catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog(typeof(ExampleWindow).Assembly));
			_container = new CompositionContainer(catalog);
			try
			{
				_container.SatisfyImportsOnce(this);
			}
			catch (CompositionException e)
			{
				Console.WriteLine(e.ToString());
			}

			gameWindow.TargetUpdateFrequency = updateRate;
			gameWindow.TargetRenderFrequency = updateRate;
			gameWindow.VSync = VSyncMode.On;
			//register callback for resizing of window
			gameWindow.Resize += GameWindow_Resize;
			//register callback for keyboard
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.KeyDown += (sender, e) => { if (Key.Escape == e.Key) gameWindow.Exit(); };
			ResourceManager = resourceProvider as ResourceManager;
		}

		public IGameWindow GameWindow { get { return gameWindow; } }

		public event Action Render;
		public IRenderContext RenderContext { get; private set; }

		public delegate void ResizeHandler(int width, int height);
		public event ResizeHandler Resize;

		public delegate void UpdateHandler(float updatePeriod);
		public event UpdateHandler Update;

		public ResourceManager ResourceManager { get; private set; }

		public System.Numerics.Vector2 CalcNormalized(int pixelX, int pixelY)
		{
			return new System.Numerics.Vector2(pixelX / (gameWindow.Width - 1f), 1f - pixelY / (gameWindow.Height - 1f));
		}

		public void Run()
		{
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += (sender, e) => Update?.Invoke((float)gameWindow.TargetUpdatePeriod);
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += (sender, e) => GameWindowRender();
			//run the update loop, which calls our registered callbacks
			gameWindow.Run();
		}

		private CompositionContainer _container;
		private GameWindow gameWindow;

		[Import]
		private IResourceProvider resourceProvider = null;

		private void GameWindowRender()
		{
			ResourceManager?.CheckForShaderChange();
			//render
			Render?.Invoke();
			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.SwapBuffers();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					gameWindow.Exit();
					break;
				case Key.F11:
					gameWindow.WindowState = WindowState.Fullscreen == gameWindow.WindowState ? WindowState.Normal : WindowState.Fullscreen;
					break;
			}
		}

		private void GameWindow_Resize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			Resize?.Invoke(gameWindow.Width, gameWindow.Height);
		}
	}
}
