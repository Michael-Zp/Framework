using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Zenseless.Application
{
	/// <summary>
	/// 
	/// </summary>
	public class ExampleWindow
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExampleWindow"/> class.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="updateRate">The update rate.</param>
		public ExampleWindow(int width = 512, int height = 512, double updateRate = 60)
		{
			//var mode = new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24);
			//gameWindow = new GameWindow(width, height, mode, "", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, OpenTK.Graphics.GraphicsContextFlags.Default);
			gameWindow = new GameWindow()
			{
				Width = width, //do not set extents in the constructor, because windows 10 with enabled scale != 100% scales our given sizes in the constructor of GameWindow
				Height = height
			};
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
			gameWindow.AddDefaultExampleWindowEvents();
			ResourceManager = resourceProvider as ResourceManager;
		}

		/// <summary>
		/// Gets the game window.
		/// </summary>
		/// <value>
		/// The game window.
		/// </value>
		public IGameWindow GameWindow { get { return gameWindow; } }

		/// <summary>
		/// Occurs when [render].
		/// </summary>
		public event Action Render;
		/// <summary>
		/// Gets the render context.
		/// </summary>
		/// <value>
		/// The render context.
		/// </value>
		public IRenderContext RenderContext { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public delegate void ResizeHandler(int width, int height);
		/// <summary>
		/// Occurs when [resize].
		/// </summary>
		public event ResizeHandler Resize;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="updatePeriod">The update period.</param>
		public delegate void UpdateHandler(float updatePeriod);
		/// <summary>
		/// Occurs when [update].
		/// </summary>
		public event UpdateHandler Update;

		/// <summary>
		/// Gets the resource manager.
		/// </summary>
		/// <value>
		/// The resource manager.
		/// </value>
		public ResourceManager ResourceManager { get; private set; }

		/// <summary>
		/// Runs the window loop, which in turn calls the registered event handlers
		/// </summary>
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
		[Import] private IResourceProvider resourceProvider = null;

		private void GameWindowRender()
		{
			ResourceManager?.CheckForShaderChange();
			//render
			Render?.Invoke();
			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.SwapBuffers();
		}

		/// <summary>
		/// Handles the Resize event of the GameWindow control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void GameWindow_Resize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			Resize?.Invoke(gameWindow.Width, gameWindow.Height);
		}
	}
}
