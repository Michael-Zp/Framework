using DMS.Sound;
using DMS.System;
using System;
using System.IO;

namespace MvcSpaceInvaders
{
	public class Sound : Disposable
	{
		public Sound()
		{
			soundEngine = new AudioPlaybackEngine();
		}

		public void Background()
		{
			//soundEngine.PlaySound(@"D:\temp\music\new\Jamie xx [None] - 00 - You've Got the Love.mp3");
		}

		public void DestroyEnemy()
		{
			//var memStream = new MemoryStream(Resourcen.EVAXDaughters);
			//soundEngine.PlaySound(memStream);
		}

		public void Lost()
		{
		}

		public void Shoot()
		{
			soundEngine.PlaySound(Resourcen.laser);
		}

		protected override void DisposeResources()
		{
			soundEngine.Dispose();
		}

		private AudioPlaybackEngine soundEngine;
	}
}
