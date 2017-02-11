using DMS.Sound;
using System;

namespace MvcSpaceInvaders
{
	public class Sound : IDisposable
	{
		public Sound()
		{
			soundEngine = new AudioPlaybackEngine();

			//laser = engine.AddSoundSourceFromIOStream(Resourcen.laser, "laser");
			// on startup:
			//var zap = new CachedSound("zap.wav");
			//var boom = new CachedSound("boom.wav");


			//// later in the app...
			//AudioPlaybackEngine.Instance.PlaySound(zap);
			//AudioPlaybackEngine.Instance.PlaySound(boom);
			//AudioPlaybackEngine.Instance.PlaySound("crash.wav");

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
			//engine.Play2D(laser, false, false, false);
			soundEngine.PlaySound(@"D:\Daten\FH Ravensburg\Framework\CG\games\MvcSpaceInvaders\Resources\laser.wav");
		}

		private AudioPlaybackEngine soundEngine;
	}
}
