namespace MvcSpaceInvaders
{
	public class Sound2
	{
		public Sound2()
		{
			//laser = engine.AddSoundSourceFromIOStream(Resourcen.laser, "laser");
			// on startup:
			//var zap = new CachedSound("zap.wav");
			//var boom = new CachedSound("boom.wav");


			//// later in the app...
			//AudioPlaybackEngine.Instance.PlaySound(zap);
			//AudioPlaybackEngine.Instance.PlaySound(boom);
			//AudioPlaybackEngine.Instance.PlaySound("crash.wav");

			//// on shutdown
			//AudioPlaybackEngine.Instance.Dispose();
		}

		public void Background()
		{
			//engine.Play2D(@"D:\temp\music\Air [Moon Safari] - 01 - La Femme d'argent.mp3", true);
		}

		public void DestroyEnemy()
		{
		}

		public void Lost()
		{
		}

		public void Shoot()
		{
			//engine.Play2D(laser, false, false, false);
		}
	}
}
