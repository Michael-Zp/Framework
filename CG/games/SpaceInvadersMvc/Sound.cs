using Zenseless.Sound;
using Zenseless.Base;

namespace SpaceInvadersMvc
{
	/// <summary>
	/// All sounds should have the same sampling frequency, otherwise mixing them will throw an exception.
	/// </summary>
	public class Sound : Disposable
	{
		public Sound()
		{
			soundEngine = new AudioPlaybackEngine();
		}

		public void Background()
		{
			//soundEngine.PlaySound(@"sound\Jamie xx - You've Got the Love.mp3");
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
			soundEngine.PlaySound(Resources.laser);
		}

		protected override void DisposeResources()
		{
			soundEngine.Dispose();
		}

		private AudioPlaybackEngine soundEngine;
	}
}
