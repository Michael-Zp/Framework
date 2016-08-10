using IrrKlang;

namespace MvcSpaceInvaders
{
	public class Sound
	{
		public Sound()
		{
			laser = engine.AddSoundSourceFromIOStream(Resourcen.laser, "laser");
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
			engine.Play2D(laser, false, false, false);
		}

		private ISoundEngine engine = new ISoundEngine();
		private readonly ISoundSource laser;
	}
}
