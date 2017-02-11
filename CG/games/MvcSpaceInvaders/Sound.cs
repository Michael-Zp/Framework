using DMS.Sound;
using System;

namespace MvcSpaceInvaders
{
	public class Sound : IDisposable
	{
		public Sound()
		{
			soundEngine = new AudioPlaybackEngine();
		}

		public void Background()
		{
			soundEngine.PlaySound(@"D:\temp\music\new\Jamie xx [None] - 00 - You've Got the Love.mp3");
		}

		public void DestroyEnemy()
		{
		}

		public void Dispose()
		{
			soundEngine.Dispose();
		}

		public void Lost()
		{
		}

		public void Shoot()
		{
			soundEngine.PlaySound(Resourcen.laser);
		}

		private AudioPlaybackEngine soundEngine;
	}
}
