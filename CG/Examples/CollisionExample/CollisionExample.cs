using OpenTK.Input;
using System;
using Zenseless.Application;
using Zenseless.Base;

namespace Example
{
	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var time = new GameTime();
			var model = new Model(time);
			var renderer = new Renderer(time);

			window.Update += (dt) =>
			{
				var movement = Keyboard.GetState()[Key.Left] ? -1f : (Keyboard.GetState()[Key.Right] ? 1f : 0f);
				model.Update(movement, dt);
				time.NewFrame();
			};

			window.Render += () =>
			{
				renderer.Clear();
				foreach(var shape in model.Shapes) renderer.DrawShape(shape);
			};

			window.Resize += renderer.Resize;

			window.Run();
		}
	}
}