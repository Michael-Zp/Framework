using Zenseless.Application;
using OpenTK.Input;
using System;

namespace SpaceInvadersMvc
{
	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var logic = new GameLogic();
			var view = new View();
			var sound = new Sound();
			logic.OnShoot += (sender, args) => { sound.Shoot(); };
			logic.OnEnemyDestroy += (sender, args) => { sound.DestroyEnemy(); };
			logic.OnLost += (sender, args) => { sound.Lost(); };
			sound.Background();

			window.Render += () => view.DrawScreen(logic.Enemies, logic.Bullets, logic.Player);
			window.Update += (dt) => Update(logic);
			window.Run();
		}

		private static void Update(GameLogic logic)
		{
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];
			logic.Update(axisLeftRight, shoot);
		}
	}
}