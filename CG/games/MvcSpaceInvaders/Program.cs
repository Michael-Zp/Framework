﻿using Zenseless.Application;
using OpenTK.Input;
using System;
using Zenseless.Base;

namespace MvcSpaceInvaders
{
	class Controller
	{
		public Controller()
		{
			logic = new GameLogic();
			view = new View();
			sound = new Sound();
			logic.OnShoot += (sender, args) => { sound.Shoot(); };
			logic.OnEnemyDestroy += (sender, args) => { sound.DestroyEnemy(); };
			logic.OnLost += (sender, args) => { sound.Lost(); };
			sound.Background();
		}

		private void Render()
		{
			view.DrawScreen(logic.Enemies, logic.Bullets, logic.Player);
		}

		private void Update(float totalTime)
		{
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];
			logic.Update(totalTime, axisLeftRight, shoot);
		}

		private GameLogic logic;
		private View view;
		private Sound sound;

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var controller = new Controller();
			var time = new GameTime();
			window.Render += controller.Render;
			window.Update += (dt) => controller.Update(time.Seconds);
			window.Run();
		}
	}
}